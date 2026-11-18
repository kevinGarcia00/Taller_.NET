namespace Application.Dtos;

public sealed class NodoListadoDto
{
    public int Id { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public string UbicacionParque { get; init; } = string.Empty;
    public DateTime FechaCreacion { get; init; }
    public decimal EnergiaGenerada { get; init; }
    public int? InversorId { get; init; }
    public string? NombreInversor { get; init; }
}
