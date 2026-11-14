namespace Taller_.NET.Modulos.Historico.Domain.Excepciones
{
    public class HistoricoInvalidoException : Exception
    {
        public HistoricoInvalidoException(string mensaje) : base(mensaje)
        {
        }
    }
}
