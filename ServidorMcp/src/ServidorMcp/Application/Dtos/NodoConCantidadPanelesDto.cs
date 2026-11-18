namespace Application.Dtos;

public sealed class NodoConCantidadPanelesDto
{
    public int Id { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public decimal EnergiaGenerada { get; init; }
    public int CantidadPaneles { get; init; }
}
