namespace Taller_.NET.Modulos.Chat.Infrastructure
{
    public sealed class OpcionesChat
    {
        // Nombre de la sección de appsettings.json que contiene estas opciones.
        public const string Seccion = "Chat";
        
        // Ejecutable de la CLI de Antigravity. Por defecto "agy" (tiene que estar declarado en el PATH)
        public string Comando { get; set; } = "agy";

        //Directorio de trabajo desde el que se lanza Antigravity. Se resuelve
        // automaticamente a la raiz del repo, sin depender de la ruta de cada usuario.
        public string DirectorioTrabajo { get; set; } = ObtenerRaizDelRepositorio();

        //Tiempo para que levante agy desde que lanza el chat el sistema
        public int SegundosEspera { get; set; } = 120;

        // URL del servidor MCP de SolarUY. El chat la consulta antes de cada respuesta.
        public string UrlMcp { get; set; } = "http://127.0.0.1:5285/mcp";

        // Nombre con el que agy conoce al servidor MCP (agy mcp list).
        public string NombreServidorMcp { get; set; } = "SolarUy";

        // Tiempo maximo para consultar tools/list al servidor MCP.
        public int SegundosEsperaMcp { get; set; } = 5;




        // Busca la raiz del repo subiendo desde la carpeta donde corre el ejecutable.
        private static string ObtenerRaizDelRepositorio()
        {
            // Arranca en bin/Debug/net10.0 (donde esta el DLL en ejecucion).
            var directorio = new DirectoryInfo(AppContext.BaseDirectory);

            // Sube por los padres hasta llegar a la raiz del filesystem.
            while (directorio is not null)
            {
                // Es la raiz si tiene .git o si contiene ambos proyectos del taller.
                var esRaiz = Directory.Exists(Path.Combine(directorio.FullName, ".git"))
                    || (Directory.Exists(Path.Combine(directorio.FullName, "Taller_.NET"))
                        && Directory.Exists(Path.Combine(directorio.FullName, "ServidorMcp")));

                // Si la encontro, esa ruta sera el DirectorioTrabajo de agy.
                if (esRaiz)
                {
                    return directorio.FullName;
                }

                // Si no, sigue subiendo un nivel.
                directorio = directorio.Parent;
            }

            // Si no encontro nada, usa la carpeta del ejecutable.
            return AppContext.BaseDirectory;
        }
    }
}
