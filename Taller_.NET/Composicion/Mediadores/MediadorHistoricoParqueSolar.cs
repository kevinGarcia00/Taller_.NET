using Microsoft.EntityFrameworkCore;
using Taller_.NET.Modulos.Historico.Application.Dtos;
using Taller_.NET.Modulos.Historico.Application.Puertos;
using Taller_.NET.Modulos.Historico.Domain;
using Taller_.NET.Modulos.ParqueSolar.Infrastructure.Persistencia;

namespace Taller_.NET.Composicion.Mediadores
{
    /// <summary>
    /// Adaptador del canal Historico -> ParqueSolar. Vive fuera de
    /// "Taller_.NET.Modulos" porque es el unico punto autorizado a conocer
    /// ambos modulos (punto de composicion).
    /// </summary>
    public class MediadorHistoricoParqueSolar : IMediadorHistoricoParqueSolar
    {
        private readonly IParqueDbContext _contexto;

        public MediadorHistoricoParqueSolar(IParqueDbContext contexto)
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

        public async Task<ContextoPanelLecturaDto?> ObtenerContextoAsync(
            int panelId,
            int nodoId,
            int parqueSolarId,
            CancellationToken cancellationToken = default)
        {
            var fila = await (
                from panel in _contexto.Paneles.AsNoTracking()
                join nodo in _contexto.Nodos.AsNoTracking() on panel.NodoId equals nodo.Id
                join parque in _contexto.Parques.AsNoTracking() on nodo.ParqueSolarId equals parque.Id
                where panel.Id == panelId && nodo.Id == nodoId && parque.Id == parqueSolarId
                select new
                {
                    NombrePanel = panel.Nombre,
                    NombreNodo = nodo.Nombre,
                    NombreParque = parque.Nombre,
                    panel.Estado
                }).FirstOrDefaultAsync(cancellationToken);

            if (fila is null || !Enum.TryParse<EstadoPanelRegistrado>(fila.Estado.ToString(), out var estado))
            {
                return null;
            }

            return new ContextoPanelLecturaDto
            {
                NombreParque = fila.NombreParque,
                NombreNodo = fila.NombreNodo,
                NombrePanel = fila.NombrePanel,
                Estado = estado
            };
        }
    }
}
