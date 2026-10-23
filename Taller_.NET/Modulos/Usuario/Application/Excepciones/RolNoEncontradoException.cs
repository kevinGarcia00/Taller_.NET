namespace Taller_.NET.Modulos.Usuario.Application.Excepciones
{
    public class RolNoEncontradoException : Exception
    {
        public RolNoEncontradoException(IEnumerable<long> rolesIds)
            : base($"Los roles solicitados no estan registrados: {string.Join(", ", rolesIds)}.")
        {
        }
    }
}
