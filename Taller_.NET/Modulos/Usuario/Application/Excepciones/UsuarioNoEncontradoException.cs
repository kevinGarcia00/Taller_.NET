namespace Taller_.NET.Modulos.Usuario.Application.Excepciones
{
    public class UsuarioNoEncontradoException : Exception
    {
        public UsuarioNoEncontradoException(Guid id)
            : base($"No existe un usuario registrado con el identificador '{id}'.")
        {
        }
    }
}
