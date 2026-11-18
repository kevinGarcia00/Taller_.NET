namespace Application.Dtos;

public sealed class ParqueListadoDto
{
    public int Id { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public string Ubicacion { get; init; } = string.Empty;
    public int CantidadNodosActuales { get; init; }
    public int CantMaxNodos { get; init; }
}
