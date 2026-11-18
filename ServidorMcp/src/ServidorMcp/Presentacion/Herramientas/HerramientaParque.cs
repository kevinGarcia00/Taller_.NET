using System.ComponentModel;
using System.Text.Json;
using ModelContextProtocol.Server;
using Application.Puertos;

namespace Presentacion.Herramientas;

[McpServerToolType]
public sealed class HerramientaParque
{
    private static readonly JsonSerializerOptions OpcionesJson = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    private readonly IParqueConsultaService _parqueConsultaService;

    public HerramientaParque(IParqueConsultaService parqueConsultaService)
    {
        _parqueConsultaService = parqueConsultaService;
    }

    [McpServerTool(Name = "listar_parques_solares"), Description("Responde '¿Qué parques solares hay registrados y en qué ubicación están?'. Devuelve nombre, ubicación, cantidad de nodos actuales y capacidad máxima de cada parque.")]
    public async Task<string> ListarParquesSolares()
    {
        return Serializar(await _parqueConsultaService.ObtenerParquesAsync());
    }

    [McpServerTool(Name = "obtener_detalle_parque_solar"), Description("Responde 'Muéstrame el detalle del parque solar X'. Busca el parque por su nombre y devuelve nombre, ubicación, cantidad de nodos, capacidad máxima, fecha de creación y la lista de nodos con su energía generada.")]
    public async Task<string> ObtenerDetalleParqueSolar(
        [Description("Nombre del parque solar a consultar, por ejemplo 'Parque2'.")] string nombre)
    {
        return Serializar(await _parqueConsultaService.ObtenerDetallePorNombreAsync(nombre));
    }

    [McpServerTool(Name = "listar_nodos_parque_solar"), Description("Responde '¿Qué nodos tiene el parque solar X?'. Busca el parque por su nombre y devuelve sus nodos con la energía generada por cada uno.")]
    public async Task<string> ListarNodosParqueSolar(
        [Description("Nombre del parque solar cuyos nodos se quieren consultar, por ejemplo 'Parque'.")] string nombre)
    {
        return Serializar(await _parqueConsultaService.ObtenerNodosPorNombreAsync(nombre));
    }

    [McpServerTool(Name = "listar_nodos_ordenados_por_energia"), Description("Responde 'Listame los nodos ordenados por energía generada acumulada, de mayor a menor'. Requiere el nombre del parque solar.")]
    public async Task<string> ListarNodosOrdenadosPorEnergia(
        [Description("Nombre del parque solar, por ejemplo 'Parque'.")] string nombre)
    {
        return Serializar(await _parqueConsultaService.ObtenerNodosOrdenadosPorEnergiaAsync(nombre));
    }

    [McpServerTool(Name = "listar_nodos_con_cantidad_paneles"), Description("Responde '¿Qué nodos pertenecen al parque seleccionado y cuántos paneles tiene cada uno?'. Devuelve cada nodo con su energía generada y su cantidad de paneles.")]
    public async Task<string> ListarNodosConCantidadPaneles(
        [Description("Nombre del parque solar, por ejemplo 'Parque'.")] string nombre)
    {
        return Serializar(await _parqueConsultaService.ObtenerNodosConCantidadPanelesAsync(nombre));
    }

    [McpServerTool(Name = "listar_paneles_del_nodo"), Description("Responde 'Muéstrame los paneles del nodo Y del parque X'. Devuelve nombre, estado, si vuelca energía y potencia de cada panel. El parque es opcional: si se omite, se busca el nodo en todos los parques.")]
    public async Task<string> ListarPanelesDelNodo(
        [Description("Nombre del nodo, por ejemplo 'Nodo1'.")] string nombreNodo,
        [Description("Nombre del parque solar. Opcional; si se omite se busca el nodo en todos los parques.")] string? nombreParque = null)
    {
        return Serializar(await _parqueConsultaService.ObtenerPanelesAsync(nombreNodo, nombreParque));
    }

    [McpServerTool(Name = "contar_paneles_del_nodo"), Description("Responde '¿Cuántos paneles tiene registrados el nodo Y del parque X?'. Devuelve la cantidad como número. El parque es opcional.")]
    public async Task<string> ContarPanelesDelNodo(
        [Description("Nombre del nodo, por ejemplo 'Nodo1'.")] string nombreNodo,
        [Description("Nombre del parque solar. Opcional.")] string? nombreParque = null)
    {
        return Serializar(await _parqueConsultaService.ContarPanelesAsync(nombreNodo, nombreParque));
    }

    [McpServerTool(Name = "listar_paneles_apagados_del_parque"), Description("Responde '¿Qué paneles del parque solar X están apagados?'. Recorre los nodos del parque y devuelve los paneles cuyo estado es Apagado.")]
    public async Task<string> ListarPanelesApagadosDelParque(
        [Description("Nombre del parque solar, por ejemplo 'Parque'.")] string nombreParque)
    {
        return Serializar(await _parqueConsultaService.ObtenerPanelesApagadosAsync(nombreParque));
    }

    [McpServerTool(Name = "obtener_panel_por_nombre"), Description("Responde '¿Cuál es el estado actual, la potencia y la vida útil del panel X?'. Busca el panel por nombre en todos los parques; opcionalmente se puede acotar por parque y nodo.")]
    public async Task<string> ObtenerPanelPorNombre(
        [Description("Nombre del panel a consultar, por ejemplo 'Panel 12'.")] string nombrePanel,
        [Description("Nombre del parque solar para acotar la búsqueda. Opcional.")] string? nombreParque = null,
        [Description("Nombre del nodo para acotar la búsqueda. Opcional.")] string? nombreNodo = null)
    {
        return Serializar(await _parqueConsultaService.ObtenerPanelPorNombreAsync(
            nombrePanel,
            nombreParque,
            nombreNodo));
    }

    [McpServerTool(Name = "obtener_inversor_del_nodo"), Description("Responde '¿Qué inversor tiene el nodo Y del parque X?'. Devuelve nombre, fecha de creación, nodo y parque al que pertenece. El parque es opcional.")]
    public async Task<string> ObtenerInversorDelNodo(
        [Description("Nombre del nodo, por ejemplo 'Nodo1'.")] string nombreNodo,
        [Description("Nombre del parque solar. Opcional.")] string? nombreParque = null)
    {
        return Serializar(await _parqueConsultaService.ObtenerInversorDelNodoAsync(nombreNodo, nombreParque));
    }

    [McpServerTool(Name = "listar_inversores"), Description("Responde '¿Qué inversores hay registrados?'. Devuelve el listado de inversores del módulo ParqueSolar.")]
    public async Task<string> ListarInversores()
    {
        return Serializar(await _parqueConsultaService.ObtenerInversoresAsync());
    }

    private static string Serializar(object resultado)
    {
        return JsonSerializer.Serialize(resultado, OpcionesJson);
    }
}
