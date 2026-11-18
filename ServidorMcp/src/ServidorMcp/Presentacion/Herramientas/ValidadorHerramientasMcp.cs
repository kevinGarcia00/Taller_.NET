using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;

namespace Presentacion.Herramientas
{
    // Verifica que el servidor exponga datos unicamente a traves de herramientas MCP.
    // Si no hay herramientas registradas, avisa en el log y evita que el servidor arranque "vacio".
    public static class ValidadorHerramientasMcp
    {
        // Clases que se registraron con WithTools<T>() en Program.cs.
        private static readonly Type[] TiposHerramienta =
        [
            typeof(HerramientaParque),
            typeof(HerramientaUsuario)
        ];

        public static void Validar(IServiceProvider servicios, ILogger logger)
        {
            // Herramientas realmente registradas en el contenedor de DI.
            var registradas = servicios.GetServices<McpServerTool>().ToArray();

            // Nombres declarados con [McpServerTool], usados solo para el aviso.
            var declaradas = ObtenerNombresDeclarados().ToArray();

            if (registradas.Length == 0 || declaradas.Length == 0)
            {
                const string aviso = "No hay herramientas MCP disponibles. El servidor no debe responder datos hasta que se registren herramientas con [McpServerTool].";

                logger.LogCritical(aviso);
                throw new InvalidOperationException(aviso);
            }

            logger.LogInformation(
                "Fuente de datos: exclusivamente herramientas MCP");
            logger.LogInformation(
                "Herramientas MCP disponibles: {Cantidad} - {Nombres}",
                registradas.Length,
                string.Join(", ", declaradas));
        }

        private static IEnumerable<string> ObtenerNombresDeclarados()
        {
            foreach (var tipo in TiposHerramienta)
            {
                var metodos = tipo.GetMethods(
                    BindingFlags.Public
                    | BindingFlags.NonPublic
                    | BindingFlags.Instance
                    | BindingFlags.Static
                    | BindingFlags.DeclaredOnly);

                foreach (var metodo in metodos)
                {
                    var atributo = metodo.GetCustomAttribute<McpServerToolAttribute>(inherit: false);

                    if (atributo is null)
                    {
                        continue;
                    }

                    yield return string.IsNullOrWhiteSpace(atributo.Name)
                        ? metodo.Name
                        : atributo.Name;
                }
            }
        }
    }
}
