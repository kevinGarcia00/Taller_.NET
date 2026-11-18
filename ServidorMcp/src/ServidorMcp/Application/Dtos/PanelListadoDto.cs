namespace Application.Dtos;

public sealed class PanelListadoDto
{
    public int Id { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public string Estado { get; init; } = string.Empty;
    public bool VolcarEnergia { get; init; }
    public decimal Potencia { get; init; }
}
