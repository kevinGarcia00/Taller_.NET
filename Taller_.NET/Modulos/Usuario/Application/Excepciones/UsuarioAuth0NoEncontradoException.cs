namespace Taller_.NET.Modulos.Usuario.Application.Excepciones
{
    public class UsuarioAuth0NoEncontradoException : Auth0IntegracionException
    {
        public UsuarioAuth0NoEncontradoException(string auth0UserId)
            : base($"El usuario '{auth0UserId}' no existe en Auth0.")
        {
        }
    }
}
