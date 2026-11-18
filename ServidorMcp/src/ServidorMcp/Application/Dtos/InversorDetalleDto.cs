namespace Application.Dtos;

public sealed class InversorDetalleDto
{
    public int Id { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public DateTime FechaCreacion { get; init; }
    public int NodoId { get; init; }
    public int ParqueSolarId { get; init; }
}
