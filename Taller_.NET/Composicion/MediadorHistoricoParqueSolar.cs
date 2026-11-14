using Microsoft.EntityFrameworkCore;
using Taller_.NET.Modulos.Historico.Application.Dtos;
using Taller_.NET.Modulos.Historico.Application.Puertos;
using Taller_.NET.Modulos.ParqueSolar.Infrastructure.Persistencia;

namespace Taller_.NET.Composicion
{
    /// <summary>
    /// Adaptador del canal Historico -> ParqueSolar. Vive fuera de
    /// "Taller_.NET.Modulos" porque es el unico punto autorizado a conocer
    /// ambos modulos (punto de composicion).
    /// </summary>
    public class MediadorHistoricoParqueSolar : IMediadorHistoricoParqueSolar
    {
        private readonly ParqueDbContext _contexto;

        public MediadorHistoricoParqueSolar(ParqueDbContext contexto)
        {
            _contexto = contexto;
        }

        public async Task<IReadOnlyList<EnergiaPanelDto>> ObtenerEnergiaTotalPanelesAsync(
            CancellationToken cancellationToken = default)
        {
            return await (from panel in _contexto.Paneles
                          join nodo in _contexto.Nodos on panel.NodoId equals nodo.Id
                          orderby panel.Id
                          select new EnergiaPanelDto
                          {
                              PanelId = panel.Id,
                              NodoId = nodo.Id,
                              ParqueSolarId = nodo.ParqueSolarId,
                              EnergiaTotal = nodo.EnergiaGenerada
                          }).ToListAsync(cancellationToken);
        }

        public async Task<bool> ExistePanelAsync(
            int panelId,
            int nodoId,
            int parqueSolarId,
            CancellationToken cancellationToken = default)
        {
            return await (from panel in _contexto.Paneles
                          join nodo in _contexto.Nodos on panel.NodoId equals nodo.Id
                          where panel.Id == panelId
                              && nodo.Id == nodoId
                              && nodo.ParqueSolarId == parqueSolarId
                          select panel.Id).AnyAsync(cancellationToken);
        }
    }
}
