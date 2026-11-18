namespace Application.Excepciones;

public sealed class PanelNoEncontradoException : Exception
{
    public PanelNoEncontradoException(string nombre)
        : base($"No se encontro un panel con el nombre '{nombre}'.")
    {
    }
}
