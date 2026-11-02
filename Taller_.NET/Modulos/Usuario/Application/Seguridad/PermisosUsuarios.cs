namespace Taller_.NET.Modulos.Usuario.Application.Seguridad
{
    /// <summary>
    /// Permisos y politicas de autorizacion del modulo Usuario. Los permisos son
    /// administrados en Auth0 y llegan en el claim "permissions" del Access Token.
    /// </summary>
    public static class PermisosUsuarios
    {
        public const string TipoClaim = "permissions";

        public const string Leer = "users:read";
        public const string Crear = "users:create";
        public const string Actualizar = "users:update";
        public const string Eliminar = "users:delete";

        public const string PoliticaLeer = "Usuarios:Leer";
        public const string PoliticaCrear = "Usuarios:Crear";
        public const string PoliticaActualizar = "Usuarios:Actualizar";
        public const string PoliticaEliminar = "Usuarios:Eliminar";

        public static readonly IReadOnlyList<string> Todos = new[]
        {
            Leer,
            Crear,
            Actualizar,
            Eliminar
        };
    }
}
