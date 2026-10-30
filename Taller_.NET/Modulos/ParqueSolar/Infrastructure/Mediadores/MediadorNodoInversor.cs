using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Taller_.NET.Modulos.ParqueSolar.Application.Dtos;
using Taller_.NET.Modulos.ParqueSolar.Application.Puertos;
using Taller_.NET.Modulos.ParqueSolar.Infrastructure.Persistencia;

namespace Taller_.NET.Modulos.ParqueSolar.Infrastructure.Mediadores
{
    public class MediadorNodoInversor : IMediadorNodoInversor
    {
        private readonly IParqueDbContext _contexto;

        public MediadorNodoInversor(IParqueDbContext contexto)
        {
            _contexto = contexto;
        }

        public async Task<bool> ExisteInversorAsync(
            int inversorId,
            CancellationToken cancellationToken = default)
        {
            return await _contexto.Inversores
                .AnyAsync(inversor => inversor.Id == inversorId, cancellationToken);
        }

        public async Task<IReadOnlyList<InversorDisponibleDto>> ObtenerDisponiblesAsync(
            CancellationToken cancellationToken = default)
        {
            return await _contexto.Inversores
                .Where(inversor => !_contexto.Nodos.Any(nodo => nodo.InversorId == inversor.Id))
                .OrderBy(inversor => inversor.Nombre)
                .Select(inversor => new InversorDisponibleDto
                {
                    Id = inversor.Id,
                    Nombre = inversor.Nombre
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyDictionary<int, string>> ObtenerNombresPorIdsAsync(
            IReadOnlyCollection<int> inversorIds,
            CancellationToken cancellationToken = default)
        {
            var ids = inversorIds.Distinct().ToList();

            if (ids.Count == 0)
            {
                return new Dictionary<int, string>();
            }

            return await _contexto.Inversores
                .Where(inversor => ids.Contains(inversor.Id))
                .ToDictionaryAsync(inversor => inversor.Id, inversor => inversor.Nombre, cancellationToken);
        }

        public async Task<IReadOnlyList<InversorOpcionDto>> ObtenerOpcionesParaNodoAsync(
            int? inversorActualId,
            CancellationToken cancellationToken = default)
        {
            return await _contexto.Inversores
                .Where(inversor =>
                    (inversorActualId.HasValue && inversor.Id == inversorActualId.Value) ||
                    !_contexto.Nodos.Any(nodo => nodo.InversorId == inversor.Id))
                .OrderBy(inversor => inversor.Nombre)
                .Select(inversor => new InversorOpcionDto
                {
                    Id = inversor.Id,
                    Nombre = inversor.Nombre
                })
                .ToListAsync(cancellationToken);
        }
    }
}
