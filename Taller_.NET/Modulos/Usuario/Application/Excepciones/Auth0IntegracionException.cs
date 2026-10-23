namespace Taller_.NET.Modulos.Usuario.Application.Excepciones
{
    public class Auth0IntegracionException : Exception
    {
        public Auth0IntegracionException(string mensaje) : base(mensaje)
        {
        }

        public Auth0IntegracionException(string mensaje, Exception innerException) : base(mensaje, innerException)
        {
        }
    }
}
