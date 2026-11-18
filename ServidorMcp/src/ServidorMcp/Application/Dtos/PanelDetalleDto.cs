namespace Application.Dtos;

public sealed class PanelDetalleDto
{
    public int Id { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public string Estado { get; init; } = string.Empty;
    public bool VolcarEnergia { get; init; }
    public decimal Potencia { get; init; }
    public string VidaUtil { get; init; } = string.Empty;
    public DateTime FechaCreacion { get; init; }
    public int NodoId { get; init; }
    public int ParqueSolarId { get; init; }
}
