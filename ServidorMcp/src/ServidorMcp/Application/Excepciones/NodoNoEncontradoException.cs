namespace Application.Excepciones;

public sealed class NodoNoEncontradoException : Exception
{
    public NodoNoEncontradoException(string nombre)
        : base($"No se encontro un nodo con el nombre '{nombre}'.")
    {
    }
}
