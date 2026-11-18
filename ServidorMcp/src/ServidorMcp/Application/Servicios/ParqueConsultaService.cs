using Application.Dtos;
using Application.Excepciones;
using Application.Puertos;
using Domain.Parques;

namespace Application.Servicios;

public sealed class ParqueConsultaService : IParqueConsultaService
{
    private const string EstadoPanelApagado = "Apagado";

    private readonly IParqueSolarConsultaRepository _repositorio;

    public ParqueConsultaService(IParqueSolarConsultaRepository repositorio)
    {
        _repositorio = repositorio;
    }

    public Task<IReadOnlyList<ParqueListadoDto>> ObtenerParquesAsync(
        CancellationToken cancellationToken = default)
    {
        return _repositorio.ObtenerParquesAsync(cancellationToken);
    }

    public async Task<ParqueDetalleDto> ObtenerDetallePorNombreAsync(
        string nombre,
        CancellationToken cancellationToken = default)
    {
        var parque = await ObtenerParquePorNombreAsync(nombre, cancellationToken);

        return await _repositorio.ObtenerDetalleParqueAsync(parque.Id, cancellationToken)
            ?? throw new ParqueNoEncontradoException(nombre.Trim());
    }

    public async Task<ParqueListadoDto> ObtenerConMayorCapacidadAsync(
        CancellationToken cancellationToken = default)
    {
        var parques = await _repositorio.ObtenerParquesAsync(cancellationToken);

        if (parques.Count == 0)
        {
            throw new SinParquesRegistradosException();
        }

        var mayorCapacidad = parques
            .Select(parque => new Parque(
                parque.Id,
                parque.Nombre,
                parque.Ubicacion,
                DateTime.UtcNow,
                parque.CantMaxNodos))
            .Aggregate((actual, candidato) =>
                candidato.TieneMayorCapacidadQue(actual) ? candidato : actual);

        return parques.Single(parque => parque.Id == mayorCapacidad.Id);
    }

    public async Task<IReadOnlyList<NodoResumenDto>> ObtenerNodosPorNombreAsync(
        string nombre,
        CancellationToken cancellationToken = default)
    {
        var parque = await ObtenerParquePorNombreAsync(nombre, cancellationToken);
        var nodos = await _repositorio.ObtenerNodosPorParqueAsync(parque.Id, cancellationToken);

        return nodos.Select(MapearNodo).ToList();
    }

    public async Task<IReadOnlyList<NodoResumenDto>> ObtenerNodosOrdenadosPorEnergiaAsync(
        string nombre,
        CancellationToken cancellationToken = default)
    {
        var nodos = await ObtenerNodosPorNombreAsync(nombre, cancellationToken);

        return nodos
            .OrderByDescending(nodo => nodo.EnergiaGenerada)
            .ToList();
    }

    public async Task<IReadOnlyList<NodoConCantidadPanelesDto>> ObtenerNodosConCantidadPanelesAsync(
        string nombre,
        CancellationToken cancellationToken = default)
    {
        var parque = await ObtenerParquePorNombreAsync(nombre, cancellationToken);
        var nodos = await _repositorio.ObtenerNodosPorParqueAsync(parque.Id, cancellationToken);
        var resultado = new List<NodoConCantidadPanelesDto>();

        foreach (var nodo in nodos)
        {
            var paneles = await _repositorio.ObtenerPanelesPorNodoAsync(
                parque.Id,
                nodo.Id,
                cancellationToken);

            resultado.Add(new NodoConCantidadPanelesDto
            {
                Id = nodo.Id,
                Nombre = nodo.Nombre,
                EnergiaGenerada = nodo.EnergiaGenerada,
                CantidadPaneles = paneles.Count
            });
        }

        return resultado;
    }

    public async Task<IReadOnlyList<PanelListadoDto>> ObtenerPanelesAsync(
        string nombreNodo,
        string? nombreParque = null,
        CancellationToken cancellationToken = default)
    {
        var (parque, nodo) = await ObtenerNodoAsync(
            nombreNodo,
            nombreParque,
            cancellationToken);

        return await _repositorio.ObtenerPanelesPorNodoAsync(parque.Id, nodo.Id, cancellationToken);
    }

    public async Task<int> ContarPanelesAsync(
        string nombreNodo,
        string? nombreParque = null,
        CancellationToken cancellationToken = default)
    {
        var paneles = await ObtenerPanelesAsync(nombreNodo, nombreParque, cancellationToken);
        return paneles.Count;
    }

    public async Task<IReadOnlyList<PanelListadoDto>> ObtenerPanelesApagadosAsync(
        string nombreParque,
        CancellationToken cancellationToken = default)
    {
        var parque = await ObtenerParquePorNombreAsync(nombreParque, cancellationToken);
        var nodos = await _repositorio.ObtenerNodosPorParqueAsync(parque.Id, cancellationToken);
        var apagados = new List<PanelListadoDto>();

        foreach (var nodo in nodos)
        {
            var paneles = await _repositorio.ObtenerPanelesPorNodoAsync(
                parque.Id,
                nodo.Id,
                cancellationToken);

            apagados.AddRange(paneles.Where(panel =>
                string.Equals(panel.Estado, EstadoPanelApagado, StringComparison.OrdinalIgnoreCase)));
        }

        return apagados;
    }

    public async Task<PanelDetalleDto> ObtenerPanelPorNombreAsync(
        string nombrePanel,
        string? nombreParque = null,
        string? nombreNodo = null,
        CancellationToken cancellationToken = default)
    {
        var parques = nombreParque is null
            ? await _repositorio.ObtenerParquesAsync(cancellationToken)
            : new[] { await ObtenerParquePorNombreAsync(nombreParque, cancellationToken) };

        foreach (var parque in parques)
        {
            var nodos = await _repositorio.ObtenerNodosPorParqueAsync(parque.Id, cancellationToken);
            var nodosABuscar = nombreNodo is null
                ? nodos
                : nodos.Where(nodo => Coincide(nodo.Nombre, nombreNodo)).ToList();

            foreach (var nodo in nodosABuscar)
            {
                var paneles = await _repositorio.ObtenerPanelesPorNodoAsync(
                    parque.Id,
                    nodo.Id,
                    cancellationToken);

                var panel = paneles.FirstOrDefault(panel => Coincide(panel.Nombre, nombrePanel));

                if (panel is null)
                {
                    continue;
                }

                var detalle = await _repositorio.ObtenerPanelAsync(
                    parque.Id,
                    nodo.Id,
                    panel.Id,
                    cancellationToken);

                if (detalle is not null)
                {
                    return detalle;
                }
            }
        }

        throw new PanelNoEncontradoException(nombrePanel.Trim());
    }

    public async Task<InversorDetalleDto> ObtenerInversorDelNodoAsync(
        string nombreNodo,
        string? nombreParque = null,
        CancellationToken cancellationToken = default)
    {
        var (parque, nodo) = await ObtenerNodoAsync(
            nombreNodo,
            nombreParque,
            cancellationToken);

        return await _repositorio.ObtenerInversorDelNodoAsync(parque.Id, nodo.Id, cancellationToken)
            ?? throw new InversorNoEncontradoException(nodo.Nombre);
    }

    public Task<IReadOnlyList<InversorListadoDto>> ObtenerInversoresAsync(
        CancellationToken cancellationToken = default)
    {
        return _repositorio.ObtenerInversoresAsync(cancellationToken);
    }

    private async Task<ParqueListadoDto> ObtenerParquePorNombreAsync(
        string nombre,
        CancellationToken cancellationToken)
    {
        var parques = await _repositorio.ObtenerParquesAsync(cancellationToken);

        return parques.FirstOrDefault(parque => Coincide(parque.Nombre, nombre))
            ?? throw new ParqueNoEncontradoException(nombre.Trim());
    }

    // Si no vienen el parque, busco el nodo en todos. Asi una herramienta MCP
    // sigue funcionando aunque el modelo omita el nombre del parque.
    private async Task<(ParqueListadoDto Parque, NodoListadoDto Nodo)> ObtenerNodoAsync(
        string nombreNodo,
        string? nombreParque,
        CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(nombreParque))
        {
            var parque = await ObtenerParquePorNombreAsync(nombreParque, cancellationToken);
            var nodos = await _repositorio.ObtenerNodosPorParqueAsync(parque.Id, cancellationToken);
            var nodo = nodos.FirstOrDefault(nodo => Coincide(nodo.Nombre, nombreNodo))
                ?? throw new NodoNoEncontradoException(nombreNodo.Trim());

            return (parque, nodo);
        }

        var parques = await _repositorio.ObtenerParquesAsync(cancellationToken);

        foreach (var parque in parques)
        {
            var nodos = await _repositorio.ObtenerNodosPorParqueAsync(parque.Id, cancellationToken);
            var nodo = nodos.FirstOrDefault(nodo => Coincide(nodo.Nombre, nombreNodo));

            if (nodo is not null)
            {
                return (parque, nodo);
            }
        }

        throw new NodoNoEncontradoException(nombreNodo.Trim());
    }

    private static bool Coincide(string valor, string buscado)
    {
        return string.Equals(valor.Trim(), buscado.Trim(), StringComparison.OrdinalIgnoreCase);
    }

    private static NodoResumenDto MapearNodo(NodoListadoDto nodo)
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
