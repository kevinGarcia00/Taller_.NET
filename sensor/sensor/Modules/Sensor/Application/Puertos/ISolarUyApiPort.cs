using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Modules.Sensor.Domain;

namespace Modules.Sensor.Application.Puertos
{
    public interface ISolarUyApiPort
    {
        Task<IReadOnlyList<Inversor>> ObtenerInversoresAsync(
            CancellationToken cancellationToken = default);

        Task<Inversor?> ObtenerInversorAsync(
            int idInversor,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<Nodo>> ObtenerNodosAsync(
            int idInversor,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<Panel>> ObtenerPanelesAsync(
            int idInversor,
            int idNodo,
            CancellationToken cancellationToken = default);

        Task<Panel?> ObtenerPanelAsync(
            int idInversor,
            int idNodo,
            int idPanel,
            CancellationToken cancellationToken = default);

        Task<Panel?> CambiarEstadoPanelAsync(
            int idInversor,
            int idNodo,
            int idPanel,
            AccionEstadoPanel accion,
            CancellationToken cancellationToken = default);

        Task<Panel?> CambiarVolcadoEnergiaAsync(
            int idInversor,
            int idNodo,
            int idPanel,
            bool volcarEnergia,
            CancellationToken cancellationToken = default);
    }
}
