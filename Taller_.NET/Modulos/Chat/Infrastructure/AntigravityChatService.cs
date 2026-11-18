using System.Diagnostics;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Taller_.NET.Modulos.Chat.Application.Puertos;

namespace Taller_.NET.Modulos.Chat.Infrastructure
{
    /*
     Antes de iniciar la CLI obtiene las herramientas activas del MCP y limita los permisos
     del proceso a ellas para evitar que responda consultando otras fuentes locales.
    */
    public sealed class AntigravityChatService : IChatService
    {
        // Contiene el comando, rutas, servidor MCP y límites de espera configurados para el módulo.
        private readonly OpcionesChat _opciones;

        private readonly IProveedorHerramientasMcp _proveedorHerramientas;

        private readonly ILogger<AntigravityChatService> _logger;


        public AntigravityChatService(
            IOptions<OpcionesChat> opciones,
            IProveedorHerramientasMcp proveedorHerramientas,
            ILogger<AntigravityChatService> logger)
        {
            _opciones = opciones.Value;
            _proveedorHerramientas = proveedorHerramientas;
            _logger = logger;
        }

        /*
         Consulta primero el catálogo MCP, ejecuta Antigravity con permisos solo para esas
         herramientas y devuelve su salida estándar. Si MCP no está disponible, falla sin
         invocar la CLI 
         */

        public async Task<string> ResponderAsync(
            string pregunta,
            CancellationToken cancellationToken = default)
        {
            // Rechazar la entrada aquí evita iniciar consultas HTTP o procesos sin una pregunta útil.
            if (string.IsNullOrWhiteSpace(pregunta))
            {
                throw new ArgumentException("La pregunta no puede estar vacía.", nameof(pregunta));
            }

            // La lista actual evita conceder permisos a herramientas eliminadas o que el MCP no publicó.
            IReadOnlyList<string> herramientas;
            try
            {
                herramientas = await _proveedorHerramientas.ObtenerNombresHerramientasAsync(cancellationToken);
            }
            catch (Exception excepcion) when ((excepcion is HttpRequestException
                or TaskCanceledException
                or UriFormatException
                or InvalidOperationException)
                && !cancellationToken.IsCancellationRequested)
            {
                // Fallar rápido: una respuesta sin herramientas MCP podría inventar datos del sistema.
                const string aviso = "El chat no responderá: las herramientas MCP de SolarUY no están disponibles.";

                _logger.LogCritical(excepcion, "{Aviso}", aviso);
                throw new InvalidOperationException(
                    aviso + " Iniciá el ServidorMcp y reintentá.",
                    excepcion);
            }

            _logger.LogInformation(
                "Herramientas MCP permitidas para el chat: {Cantidad} - {Nombres}",
                herramientas.Count,
                string.Join(", ", herramientas));

            // Se ejecuta sin shell para pasar cada argumento de forma independiente y evitar
            // que la pregunta o la configuración se interpreten como comandos del shell.
            var inicio = new ProcessStartInfo
            {
                FileName = _opciones.Comando,
                WorkingDirectory = _opciones.DirectorioTrabajo,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            // Además de limitar permisos, el prompt orienta al modelo a no usar archivos locales
            // ni bases de datos; así las respuestas quedan basadas en las fuentes aprobadas.
            var instruccion =
                "Respondé usando exclusivamente las herramientas MCP del servidor " + _opciones.NombreServidorMcp + ". " +
                "No leas archivos, no consultes bases de datos SQLite ni datos locales. " +
                "Si la respuesta no sale de esas herramientas, decí que no está disponible.\n\n" +
                "Pregunta: " + pregunta;

            inicio.ArgumentList.Add("--prompt");
            inicio.ArgumentList.Add(instruccion);
            inicio.ArgumentList.Add("--output-format");
            inicio.ArgumentList.Add("text");

            // La lista de permisos se construye con los nombres consultados al MCP.
            var reglas = herramientas.Select(nombre => $"mcp({_opciones.NombreServidorMcp}/{nombre})");
            var ajustes = JsonSerializer.Serialize(new { permissions = new { allow = reglas } });

            inicio.ArgumentList.Add("--settings");
            inicio.ArgumentList.Add(ajustes);

            // Libera los recursos nativos asociados al proceso cuando termina este método.
            using var proceso = new Process { StartInfo = inicio };

            if (!proceso.Start())
            {
                throw new InvalidOperationException("No se pudo iniciar el CLI de Antigravity.");
            }

            // Leer ambos pipes en paralelo evita que Antigravity se bloquee al llenar uno
            // mientras la aplicación espera que el otro termine.
            var salida = proceso.StandardOutput.ReadToEndAsync(cancellationToken);
            var error = proceso.StandardError.ReadToEndAsync(cancellationToken);


            using var espera = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            espera.CancelAfter(TimeSpan.FromSeconds(_opciones.SegundosEspera));

            try
            {
                // Se espera tanto el cierre del proceso como la lectura completa de sus dos pipes.
                await proceso.WaitForExitAsync(espera.Token);
                await Task.WhenAll(salida, error).WaitAsync(espera.Token);
            }
            catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
            {
                // Si vence el límite local, se termina también cualquier proceso hijo para
                // que no quede una CLI ejecutándose sin que la petición siga activa.
                MatarSiSigueVivo(proceso);
                throw new TimeoutException(
                    $"Antigravity no respondió en {_opciones.SegundosEspera} segundos.");
            }
            catch (OperationCanceledException)
            {
                // Una cancelación solicitada por el usuario también debe dejar el sistema limpio.
                MatarSiSigueVivo(proceso);
                throw;
            }

            // Las lecturas ya finalizaron; se recuperan sus contenidos para evaluar el resultado.
            var texto = await salida;
            var errores = await error;

            // El código de salida distingue una ejecución exitosa de un fallo de la CLI;
            // stderr conserva el diagnóstico original para que pueda investigarse.
            if (proceso.ExitCode != 0)
            {
                throw new InvalidOperationException(
                    $"Antigravity terminó con código {proceso.ExitCode}. {errores}".Trim());
            }

            // stderr se reserva para diagnósticos; solo stdout se entrega como respuesta al usuario.
            return texto.Trim();
        }

        /// <summary>
        /// Termina el proceso y sus descendientes si todavía siguen activos, por ejemplo
        /// después de un timeout o de una cancelación del chat.
        /// </summary>
        private static void MatarSiSigueVivo(Process proceso)
        {
            if (!proceso.HasExited)
            {
                proceso.Kill(entireProcessTree: true);
            }
        }
    }
}
