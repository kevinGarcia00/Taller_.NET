namespace Application.Excepciones;

public sealed class SinParquesRegistradosException : Exception
{
    public SinParquesRegistradosException()
        : base("No hay parques solares registrados.")
    {
    }
}
