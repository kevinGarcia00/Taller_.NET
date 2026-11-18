namespace Infrastructure.ModelosApi;

internal sealed class ParqueListadoApiModel
{
    public int Id { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public string Ubicacion { get; init; } = string.Empty;
    public int CantidadNodosActuales { get; init; }
}

internal sealed class ParqueDetalleApiModel
{
    public int Id { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public string Ubicacion { get; init; } = string.Empty;
    public int CantMaxNodos { get; init; }
    public int CantidadNodosActuales { get; init; }
    public DateTime FechaCreacion { get; init; }
    public List<NodoApiModel> ListaNodos { get; init; } = [];
}

internal sealed class NodoApiModel
{
    public int Id { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public string UbicacionParque { get; init; } = string.Empty;
    public DateTime FechaCreacion { get; init; }
    public decimal EnergiaGenerada { get; init; }
    public int? InversorId { get; init; }
    public string? NombreInversor { get; init; }
    public int ParqueSolarId { get; init; }
}

internal sealed class InversorConsultaApiModel
{
    public int Id { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public DateTime FechaCreacion { get; init; }
}

internal sealed class InversorDetalleApiModel
{
    public int Id { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public DateTime FechaCreacion { get; init; }
    public int NodoId { get; init; }
    public int ParqueSolarId { get; init; }
}

internal sealed class PanelConsultaApiModel
{
    public int Id { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public DateTime FechaCreacion { get; init; }
    public string EstadoPanel { get; init; } = string.Empty;
    public bool Encendido { get; init; }
    public bool VolcarEnergia { get; init; }
    public decimal Potencia { get; init; }
    public string VidaUtil { get; init; } = string.Empty;
    public int IdInversor { get; init; }
    public int IdNodo { get; init; }
    public int IdParqueSolar { get; init; }
}
