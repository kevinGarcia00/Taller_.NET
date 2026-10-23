namespace Taller_.NET.Modulos.Usuario.Application.Excepciones
{
    public class NoSePuedeEliminarUltimoAdministradorException : Exception
    {
        public NoSePuedeEliminarUltimoAdministradorException(string message) : base(message)
        {
        }
    }
}