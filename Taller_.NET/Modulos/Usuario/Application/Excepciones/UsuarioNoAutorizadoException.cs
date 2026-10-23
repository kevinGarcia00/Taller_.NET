namespace Taller_.NET.Modulos.Usuario.Application.Excepciones
{
    public class UsuarioNoAutorizadoException : Exception
    {
        public UsuarioNoAutorizadoException(string message) : base(message)
        {
        }
    }
}
