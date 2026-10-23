namespace Taller_.NET.Modulos.Usuario.Application.Excepciones
{
    public class CorreoUsuarioDuplicadoException : Exception
    {
        public CorreoUsuarioDuplicadoException(string correo)
            : base($"Ya existe un usuario registrado con el correo '{correo}'.")
        {
        }
    }
}
