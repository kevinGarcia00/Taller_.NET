namespace Application.Dtos;

public sealed class NodoResumenDto
{
    public int Id { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public string UbicacionParque { get; init; } = string.Empty;
    public DateTime FechaCreacion { get; init; }
    public decimal EnergiaGenerada { get; init; }
}
