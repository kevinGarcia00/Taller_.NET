using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Taller_.NET.Modulos.ParqueSolar.Domain;
using Taller_.NET.Modulos.ParqueSolar.Infrastructure.Persistencia;
using NodoDomain = Taller_.NET.Modulos.ParqueSolar.Domain.Nodo;
using PanelDomain = Taller_.NET.Modulos.ParqueSolar.Domain.Panel;

namespace Taller_.NET.Modulos.ParqueSolar.Infrastructure.Repositorios
{
    public class NodoRepository : INodoRepository
    {
        private readonly IParqueDbContext _contexto;
        private readonly IPanelRepository _panelRepository;

        public NodoRepository(IParqueDbContext contexto, IPanelRepository panelRepository)
        {
            _contexto = contexto;
            _panelRepository = panelRepository;
        }

        public async Task<bool> ExisteNodoConNombreAsync(
            string nombre,
            int parqueSolarId,
            CancellationToken cancellationToken = default)
        {
            return await _contexto.Nodos
                .AnyAsync(
                    nodo => nodo.Nombre == nombre && nodo.ParqueSolarId == parqueSolarId,
                    cancellationToken);
        }

        public async Task<bool> ExisteInversorEnUsoAsync(
            int inversorId,
            CancellationToken cancellationToken = default)
        {
            return await _contexto.Nodos
                .AnyAsync(nodo => nodo.InversorId == inversorId, cancellationToken);
        }

        public async Task<bool> ExisteInversorEnOtroNodoAsync(
            int inversorId,
            int nodoIdActual,
            CancellationToken cancellationToken = default)
        {
            return await _contexto.Nodos
                .AnyAsync(
                    nodo => nodo.InversorId == inversorId && nodo.Id != nodoIdActual,
                    cancellationToken);
        }

        public async Task<bool> ExisteOtroNodoConNombreAsync(
            string nombre,
            int parqueSolarId,
            int nodoIdActual,
            CancellationToken cancellationToken = default)
        {
            return await _contexto.Nodos
                .AnyAsync(
                    nodo => nodo.Nombre == nombre &&
                            nodo.ParqueSolarId == parqueSolarId &&
                            nodo.Id != nodoIdActual,
                    cancellationToken);
        }

        public async Task<IReadOnlyList<NodoDomain>> ObtenerPorParqueAsync(
            int parqueSolarId,
            CancellationToken cancellationToken = default)
        {
            var nodos = await _contexto.Nodos
                .AsNoTracking()
                .Where(nodo => nodo.ParqueSolarId == parqueSolarId)
                .OrderBy(nodo => nodo.Nombre)
                .ToListAsync(cancellationToken);

            await CalcularEnergiaGeneradaAsync(nodos, cancellationToken);

            return nodos;
        }

        public async Task<NodoDomain?> ObtenerPorIdAsync(
            int nodoId,
            CancellationToken cancellationToken = default)
        {
            var nodo = await _contexto.Nodos
                .FirstOrDefaultAsync(nodo => nodo.Id == nodoId, cancellationToken);

            if (nodo is not null)
            {
                await CalcularEnergiaGeneradaAsync(new[] { nodo }, cancellationToken);
            }

            return nodo;
        }

        public async Task<int> AgregarAsync(NodoDomain nodo, CancellationToken cancellationToken = default)
        {
            await _contexto.Nodos.AddAsync(nodo, cancellationToken);
            await _contexto.SaveChangesAsync(cancellationToken);

            return nodo.Id;
        }

        public async Task ActualizarAsync(NodoDomain nodo, CancellationToken cancellationToken = default)
        {
            _contexto.Nodos.Update(nodo);
            await _contexto.SaveChangesAsync(cancellationToken);
        }

        public async Task EliminarAsync(NodoDomain nodo, CancellationToken cancellationToken = default)
        {
            _contexto.Nodos.Remove(nodo);
            await _contexto.SaveChangesAsync(cancellationToken);
        }

        private async Task CalcularEnergiaGeneradaAsync(
            IReadOnlyCollection<NodoDomain> nodos,
            CancellationToken cancellationToken)
        {
            if (nodos.Count == 0)
            {
                return;
            }

            var ids = nodos.Select(nodo => nodo.Id).ToHashSet();
            var panelesPorNodo = (await _panelRepository.ObtenerTodosAsync(cancellationToken))
                .Where(panel => ids.Contains(panel.NodoId))
                .GroupBy(panel => panel.NodoId)
                .ToDictionary(
                    grupo => grupo.Key,
                    grupo => (IReadOnlyCollection<PanelDomain>)grupo.ToList());

            foreach (var nodo in nodos)
            {
                var paneles = panelesPorNodo.TryGetValue(nodo.Id, out var delNodo)
                    ? delNodo
                    : Array.Empty<PanelDomain>();

                nodo.ActualizarEnergiaGenerada(paneles);
            }
        }
    }
}
