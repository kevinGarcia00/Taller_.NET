namespace Taller_.NET.Modulos.Usuario.Application.Seguridad
{
    /// <summary>
    /// Nombres de los roles definidos en Auth0 que el backend necesita reconocer
    /// para reglas de negocio y para mantener compatibilidad con los modulos que
    /// todavia autorizan por rol.
    /// </summary>
    public static class RolesAuth0
    {
        public const string Administrador = "Administrador";
        public const string Mantenimiento = "Mantenimiento";
    }
}
