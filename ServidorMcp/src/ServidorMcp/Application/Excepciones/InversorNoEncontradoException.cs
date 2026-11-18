namespace Application.Excepciones;

public sealed class InversorNoEncontradoException : Exception
{
    public InversorNoEncontradoException(string nombreNodo)
        : base($"El nodo '{nombreNodo}' no tiene un inversor asignado.")
    {
    }
}
