using Microsoft.EntityFrameworkCore;
using Taller_.NET.Modulos.ParqueSolar.Domain;
using Taller_.NET.Modulos.ParqueSolar.Infrastructure.Persistencia;
using PanelDomain = Taller_.NET.Modulos.ParqueSolar.Domain.Panel;

namespace Taller_.NET.Modulos.ParqueSolar.Infrastructure.Repositorios
{
    public class PanelRepository : IPanelRepository
    {
        private readonly IParqueDbContext _contexto;

        public PanelRepository(IParqueDbContext contexto)
        {
            _contexto = contexto;
        }

        public async Task<bool> ExistePanelConNombreAsync(
            string nombre,
            int nodoId,
            CancellationToken cancellationToken = default)
        {
            return await _contexto.Paneles
                .AnyAsync(
                    panel => panel.Nombre == nombre && panel.NodoId == nodoId,
                    cancellationToken);
        }

        public async Task<IReadOnlyList<PanelDomain>> ObtenerPorNodoAsync(
            int nodoId,
            CancellationToken cancellationToken = default)
        {
            return await _contexto.Paneles
                .AsNoTracking()
                .Where(panel => panel.NodoId == nodoId)
                .OrderBy(panel => panel.Nombre)
                .ToListAsync(cancellationToken);
        }

        public async Task<PanelDomain?> ObtenerPorIdYNodoAsync(
            int panelId,
            int nodoId,
            CancellationToken cancellationToken = default)
        {
            return await _contexto.Paneles
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    panel => panel.Id == panelId && panel.NodoId == nodoId,
                    cancellationToken);
        }

        public async Task<bool> ExisteOtroPanelConNombreAsync(
            string nombre,
            int nodoId,
            int panelIdActual,
            CancellationToken cancellationToken = default)
        {
            return await _contexto.Paneles
                .AnyAsync(
                    panel => panel.Nombre == nombre &&
                             panel.NodoId == nodoId &&
                             panel.Id != panelIdActual,
                    cancellationToken);
        }

        public async Task<int> AgregarAsync(PanelDomain panel, CancellationToken cancellationToken = default)
        {
            await _contexto.Paneles.AddAsync(panel, cancellationToken);
            await _contexto.SaveChangesAsync(cancellationToken);

            return panel.Id;
        }

        public async Task ActualizarAsync(PanelDomain panel, CancellationToken cancellationToken = default)
        {
            var rastreado = BuscarRastreado(panel.Id);

            if (rastreado is null)
            {
                _contexto.Paneles.Update(panel);
            }
            else if (!ReferenceEquals(rastreado, panel))
            {
                rastreado.ActualizarDatos(panel.Nombre, panel.Estado, panel.Potencia, panel.VidaUtil);
            }

            await _contexto.SaveChangesAsync(cancellationToken);
        }

        public async Task EliminarAsync(PanelDomain panel, CancellationToken cancellationToken = default)
        {
            var rastreado = BuscarRastreado(panel.Id);

            if (rastreado is null)
            {
                _contexto.Paneles.Remove(panel);
            }
            else
            {
                _contexto.Paneles.Remove(rastreado);
            }

            await _contexto.SaveChangesAsync(cancellationToken);
        }

        private PanelDomain? BuscarRastreado(int panelId)
        {
            return _contexto.Paneles.Local.FirstOrDefault(panel => panel.Id == panelId);
        }
    }
}
