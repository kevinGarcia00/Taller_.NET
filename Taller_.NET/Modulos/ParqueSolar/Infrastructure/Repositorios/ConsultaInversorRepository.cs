using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Taller_.NET.Modulos.ParqueSolar.Application.Puertos;
using Taller_.NET.Modulos.ParqueSolar.Infrastructure.Persistencia;
using InversorDomain = Taller_.NET.Modulos.ParqueSolar.Domain.Inversor;
using NodoDomain = Taller_.NET.Modulos.ParqueSolar.Domain.Nodo;
using PanelDomain = Taller_.NET.Modulos.ParqueSolar.Domain.Panel;

namespace Taller_.NET.Modulos.ParqueSolar.Infrastructure.Repositorios
{
    public class ConsultaInversorRepository : IConsultaInversorRepository
    {
        private readonly IParqueDbContext _contexto;

        public ConsultaInversorRepository(IParqueDbContext contexto)
        {
            _contexto = contexto;
        }

        public async Task<IReadOnlyList<InversorDomain>> ObtenerInversoresAsync(
            CancellationToken cancellationToken = default)
        {
            return await _contexto.Inversores
                .AsNoTracking()
                .OrderBy(inversor => inversor.Nombre)
                .ToListAsync(cancellationToken);
        }

        public async Task<InversorDomain?> ObtenerInversorPorIdAsync(
            int idInversor,
            CancellationToken cancellationToken = default)
        {
            return await _contexto.Inversores
                .AsNoTracking()
                .FirstOrDefaultAsync(inversor => inversor.Id == idInversor, cancellationToken);
        }

        public async Task<IReadOnlyList<NodoDomain>> ObtenerNodosPorInversorAsync(
            int idInversor,
            CancellationToken cancellationToken = default)
        {
            return await _contexto.Nodos
                .AsNoTracking()
                .Where(nodo => nodo.InversorId == idInversor)
                .OrderBy(nodo => nodo.Nombre)
                .ToListAsync(cancellationToken);
        }

        public async Task<NodoDomain?> ObtenerNodoPorIdAsync(
            int idNodo,
            CancellationToken cancellationToken = default)
        {
            return await _contexto.Nodos
                .AsNoTracking()
                .FirstOrDefaultAsync(nodo => nodo.Id == idNodo, cancellationToken);
        }

        public async Task<IReadOnlyList<PanelDomain>> ObtenerPanelesPorNodoAsync(
            int idNodo,
            CancellationToken cancellationToken = default)
        {
            return await _contexto.Paneles
                .AsNoTracking()
                .Where(panel => panel.NodoId == idNodo)
                .OrderBy(panel => panel.Nombre)
                .ToListAsync(cancellationToken);
        }

        public async Task<PanelDomain?> ObtenerPanelPorIdYNodoAsync(
            int idPanel,
            int idNodo,
            CancellationToken cancellationToken = default)
        {
            return await _contexto.Paneles
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    panel => panel.Id == idPanel && panel.NodoId == idNodo,
                    cancellationToken);
        }
    }
}
