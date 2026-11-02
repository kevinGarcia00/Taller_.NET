namespace Taller_.NET.Modulos.Usuario.Application.Excepciones
{
    public class RolNoEncontradoException : Exception
    {
        public RolNoEncontradoException(IEnumerable<string> rolesIds)
            : base($"Los roles solicitados no estan registrados en Auth0: {string.Join(", ", rolesIds)}.")
        {
        }
    }
}
