using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Application.Dtos;
using Application.Puertos;
using Infrastructure.ModelosApi;

namespace Infrastructure.Repositorios;

public sealed class ParqueSolarHttpRepository : IParqueSolarConsultaRepository
{
    private static readonly JsonSerializerOptions OpcionesJson = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly HttpClient _httpClient;

    public ParqueSolarHttpRepository(HttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<ParqueListadoDto>> ObtenerParquesAsync(
        CancellationToken cancellationToken = default)
    {
        var parquesApi = await ObtenerAsync<List<ParqueListadoApiModel>>("api/parque", cancellationToken);
        var tareas = parquesApi.Select(async parque =>
        {
            var detalle = await ObtenerDetalleParqueAsync(parque.Id, cancellationToken);

            return new ParqueListadoDto
            {
                Id = parque.Id,
                Nombre = parque.Nombre,
                Ubicacion = parque.Ubicacion,
                CantidadNodosActuales = parque.CantidadNodosActuales,
                CantMaxNodos = detalle?.CantMaxNodos ?? 0
            };
        });

        return (await Task.WhenAll(tareas)).ToList();
    }

    public async Task<ParqueDetalleDto?> ObtenerDetalleParqueAsync(
        int parqueSolarId,
        CancellationToken cancellationToken = default)
    {
        var parque = await ObtenerOpcionalAsync<ParqueDetalleApiModel>(
            $"api/parque/{parqueSolarId}",
            cancellationToken);

        if (parque is null)
        {
            return null;
        }

        return new ParqueDetalleDto
        {
            Id = parque.Id,
            Nombre = parque.Nombre,
            Ubicacion = parque.Ubicacion,
            CantMaxNodos = parque.CantMaxNodos,
            CantidadNodosActuales = parque.CantidadNodosActuales,
            FechaCreacion = parque.FechaCreacion,
            ListaNodos = parque.ListaNodos.Select(MapearNodoResumen).ToList()
        };
    }

    public async Task<IReadOnlyList<NodoListadoDto>> ObtenerNodosPorParqueAsync(
        int parqueSolarId,
        CancellationToken cancellationToken = default)
    {
        var nodos = await ObtenerAsync<List<NodoApiModel>>(
            $"api/parque/{parqueSolarId}/nodo",
            cancellationToken);

        return nodos.Select(MapearNodoListado).ToList();
    }

    public async Task<IReadOnlyList<PanelListadoDto>> ObtenerPanelesPorNodoAsync(
        int parqueSolarId,
        int nodoId,
        CancellationToken cancellationToken = default)
    {
        var paneles = await ObtenerAsync<List<PanelConsultaApiModel>>(
            $"api/parque/{parqueSolarId}/nodo/{nodoId}/panel",
            cancellationToken);

        return paneles.Select(MapearPanelListado).ToList();
    }

    public async Task<PanelDetalleDto?> ObtenerPanelAsync(
        int parqueSolarId,
        int nodoId,
        int panelId,
        CancellationToken cancellationToken = default)
    {
        var panel = await ObtenerOpcionalAsync<PanelConsultaApiModel>(
            $"api/parque/{parqueSolarId}/nodo/{nodoId}/panel/{panelId}",
            cancellationToken);

        return panel is null ? null : MapearPanelDetalle(panel);
    }

    public async Task<InversorDetalleDto?> ObtenerInversorDelNodoAsync(
        int parqueSolarId,
        int nodoId,
        CancellationToken cancellationToken = default)
    {
        var inversor = await ObtenerOpcionalAsync<InversorDetalleApiModel>(
            $"api/parque/{parqueSolarId}/nodo/{nodoId}/inversor",
            cancellationToken);

        if (inversor is null)
        {
            return null;
        }

        return new InversorDetalleDto
        {
            Id = inversor.Id,
            Nombre = inversor.Nombre,
            FechaCreacion = inversor.FechaCreacion,
            NodoId = inversor.NodoId,
            ParqueSolarId = inversor.ParqueSolarId
        };
    }

    public async Task<IReadOnlyList<InversorListadoDto>> ObtenerInversoresAsync(
        CancellationToken cancellationToken = default)
    {
        var inversores = await ObtenerAsync<List<InversorConsultaApiModel>>(
            "api/inversor",
            cancellationToken);

        return inversores.Select(inversor => new InversorListadoDto
        {
            Id = inversor.Id,
            Nombre = inversor.Nombre,
            FechaCreacion = inversor.FechaCreacion
        }).ToList();
    }

    private async Task<T> ObtenerAsync<T>(string ruta, CancellationToken cancellationToken)
    {
        using var respuesta = await _httpClient.GetAsync(ruta, cancellationToken);
        respuesta.EnsureSuccessStatusCode();

        return await respuesta.Content.ReadFromJsonAsync<T>(OpcionesJson, cancellationToken)
            ?? throw new InvalidOperationException($"La API no devolvio datos para '{ruta}'.");
    }

    private async Task<T?> ObtenerOpcionalAsync<T>(string ruta, CancellationToken cancellationToken)
    {
        using var respuesta = await _httpClient.GetAsync(ruta, cancellationToken);

        if (respuesta.StatusCode == HttpStatusCode.NotFound)
        {
            return default;
        }

        respuesta.EnsureSuccessStatusCode();

        return await respuesta.Content.ReadFromJsonAsync<T>(OpcionesJson, cancellationToken);
    }

    private static PanelListadoDto MapearPanelListado(PanelConsultaApiModel panel)
    {
        return new PanelListadoDto
        {
            Id = panel.Id,
            Nombre = panel.Nombre,
            Estado = panel.EstadoPanel,
            VolcarEnergia = panel.VolcarEnergia,
            Potencia = panel.Potencia
        };
    }

    private static PanelDetalleDto MapearPanelDetalle(PanelConsultaApiModel panel)
    {
        return new PanelDetalleDto
        {
            Id = panel.Id,
            Nombre = panel.Nombre,
            Estado = panel.EstadoPanel,
            VolcarEnergia = panel.VolcarEnergia,
            Potencia = panel.Potencia,
            VidaUtil = panel.VidaUtil,
            FechaCreacion = panel.FechaCreacion,
            NodoId = panel.IdNodo,
            ParqueSolarId = panel.IdParqueSolar
        };
    }

    private static NodoListadoDto MapearNodoListado(NodoApiModel nodo)
    {
        return new NodoListadoDto
        {
            Id = nodo.Id,
            Nombre = nodo.Nombre,
            UbicacionParque = nodo.UbicacionParque,
            FechaCreacion = nodo.FechaCreacion,
            EnergiaGenerada = nodo.EnergiaGenerada,
            InversorId = nodo.InversorId,
            NombreInversor = nodo.NombreInversor
        };
    }

    private static NodoResumenDto MapearNodoResumen(NodoApiModel nodo)
    {
        return new NodoResumenDto
        {
            Id = nodo.Id,
            Nombre = nodo.Nombre,
            UbicacionParque = nodo.UbicacionParque,
            FechaCreacion = nodo.FechaCreacion,
            EnergiaGenerada = nodo.EnergiaGenerada
        };
    }
}
