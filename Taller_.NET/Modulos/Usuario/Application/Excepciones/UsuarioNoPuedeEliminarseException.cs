namespace Taller_.NET.Modulos.Usuario.Application.Excepciones
{
    public class UsuarioNoPuedeEliminarseException : Exception
    {
        public UsuarioNoPuedeEliminarseException(string message) : base(message)
        {
        }
    }
}