namespace Application.Dtos;

public sealed class InversorListadoDto
{
    public int Id { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public DateTime FechaCreacion { get; init; }
}
