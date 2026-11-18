namespace Application.Dtos;

public sealed class ParqueDetalleDto
{
    public int Id { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public string Ubicacion { get; init; } = string.Empty;
    public int CantMaxNodos { get; init; }
    public int CantidadNodosActuales { get; init; }
    public DateTime FechaCreacion { get; init; }
    public IReadOnlyList<NodoResumenDto> ListaNodos { get; init; } = [];
}
