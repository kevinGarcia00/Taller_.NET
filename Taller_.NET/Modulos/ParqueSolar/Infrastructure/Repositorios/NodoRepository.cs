using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Taller_.NET.Modulos.ParqueSolar.Domain;
using Taller_.NET.Modulos.ParqueSolar.Infrastructure.Persistencia;
using NodoDomain = Taller_.NET.Modulos.ParqueSolar.Domain.Nodo;

namespace Taller_.NET.Modulos.ParqueSolar.Infrastructure.Repositorios
{
    public class NodoRepository : INodoRepository
    {
        private readonly IParqueDbContext _contexto;

        public NodoRepository(IParqueDbContext contexto)
        {
            _contexto = contexto;
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
            return await _contexto.Nodos
                .AsNoTracking()
                .Where(nodo => nodo.ParqueSolarId == parqueSolarId)
                .OrderBy(nodo => nodo.Nombre)
                .ToListAsync(cancellationToken);
        }

        public async Task<NodoDomain?> ObtenerPorIdAsync(
            int nodoId,
            CancellationToken cancellationToken = default)
        {
            return await _contexto.Nodos
                .FirstOrDefaultAsync(nodo => nodo.Id == nodoId, cancellationToken);
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
    }
}
