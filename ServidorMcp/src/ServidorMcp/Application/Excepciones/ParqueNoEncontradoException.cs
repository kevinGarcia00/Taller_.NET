namespace Application.Excepciones;

public sealed class ParqueNoEncontradoException : Exception
{
    public ParqueNoEncontradoException(string nombre)
        : base($"No se encontro un parque solar con el nombre '{nombre}'.")
    {
    }
}
