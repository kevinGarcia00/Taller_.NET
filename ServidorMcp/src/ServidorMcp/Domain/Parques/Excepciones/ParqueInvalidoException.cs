namespace Domain.Parques.Excepciones;

public sealed class ParqueInvalidoException : Exception
{
    public ParqueInvalidoException(string mensaje) : base(mensaje)
    {
    }
}
