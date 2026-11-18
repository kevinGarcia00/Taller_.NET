using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Taller_.NET.Modulos.Chat.Application.Puertos;

namespace Taller_.NET.Modulos.Chat.Infrastructure
{
    /*
     Consulta las herramientas que el servidor MCP publica.
     La consulta se realiza antes de invocar Antigravity para que el permiso se base */
    public sealed class ProveedorHerramientasMcp : IProveedorHerramientasMcp
    {
        // tools/list es el método JSON-RPC de MCP que devuelve las herramientas registradas.
        private const string CuerpoSolicitud =
            "{\"jsonrpc\":\"2.0\",\"id\":1,\"method\":\"tools/list\",\"params\":{}}";

        // Cliente HTTP tipado administrado por DI, reutilizado para hablar con el endpoint MCP.
        private readonly HttpClient _cliente;

        // Opciones necesarias para la URL del servidor y el límite de tiempo de la consulta.
        private readonly OpcionesChat _opciones;

        
        // Configura el cliente HTTP tipado y aplica el timeout de descubrimiento de herramientas.
        public ProveedorHerramientasMcp(HttpClient cliente, IOptions<OpcionesChat> opciones)
        {
            _cliente = cliente;
            _opciones = opciones.Value;

            // HttpClient requiere un timeout positivo; el mínimo de un segundo evita que
            // un valor cero o negativo convierta el descubrimiento en una espera inválida.
            _cliente.Timeout = TimeSpan.FromSeconds(Math.Max(1, _opciones.SegundosEsperaMcp));
        }

    
        /// Envía tools/list al servidor MCP y extrae los nombres anunciados en su respuesta.
        public async Task<IReadOnlyList<string>> ObtenerNombresHerramientasAsync(
            CancellationToken cancellationToken = default)
        {
            using var solicitud = new HttpRequestMessage(HttpMethod.Post, _opciones.UrlMcp);

            solicitud.Headers.Accept.ParseAdd("application/json");
            solicitud.Headers.Accept.ParseAdd("text/event-stream");
            solicitud.Content = new StringContent(CuerpoSolicitud, Encoding.UTF8, "application/json");

            using var respuesta = await _cliente.SendAsync(solicitud, cancellationToken);

            respuesta.EnsureSuccessStatusCode();

            var cuerpo = await respuesta.Content.ReadAsStringAsync(cancellationToken);
            var nombres = ExtraerNombres(cuerpo);

            if (nombres.Count == 0)
            {
                // Un servidor accesible pero sin tools no permite responder bajo el modelo
                // de permisos restringidos; se informa como indisponibilidad funcional.
                throw new InvalidOperationException(
                    "El servidor MCP respondió pero no publicó ninguna herramienta.");
            }

            return nombres;
        }

        private static IReadOnlyList<string> ExtraerNombres(string cuerpo)
        {
            var nombres = new List<string>();

          
            var lineas = cuerpo.Split(
                '\n',
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            foreach (var linea in lineas)
            {
                var json = linea.StartsWith("data:", StringComparison.OrdinalIgnoreCase)
                    ? linea[5..].Trim()
                    : linea;

                if (!json.StartsWith('{'))
                {
                    continue;
                }

                using var documento = JsonDocument.Parse(json);

                if (!documento.RootElement.TryGetProperty("result", out var resultado)
                    || !resultado.TryGetProperty("tools", out var herramientas))
                {
                    continue;
                }

                foreach (var herramienta in herramientas.EnumerateArray())
                {
                    if (herramienta.TryGetProperty("name", out var nombre))
                    {
                        nombres.Add(nombre.GetString() ?? string.Empty);
                    }
                }
            }

            return nombres
                .Where(nombre => !string.IsNullOrWhiteSpace(nombre))
                .Distinct()
                .ToArray();
        }
    }
}
