using Microsoft.EntityFrameworkCore;
using Taller_.NET.Modulos.ParqueSolar.Domain;
using Taller_.NET.Modulos.ParqueSolar.Infrastructure.Persistencia;
using InversorDomain = Taller_.NET.Modulos.ParqueSolar.Domain.Inversor;

namespace Taller_.NET.Modulos.ParqueSolar.Infrastructure.Repositorios
{
    public class InversorRepository : IInversorRepository
    {
        private readonly IParqueDbContext _contexto;

        public InversorRepository(IParqueDbContext contexto)
        {
            _contexto = contexto;
        }

        public async Task<int> AgregarAsync(InversorDomain inversor, CancellationToken cancellationToken = default)
        {
            await _contexto.Inversores.AddAsync(inversor, cancellationToken);
            await _contexto.SaveChangesAsync(cancellationToken);

            return inversor.Id;
        }

        public async Task<InversorDomain?> ObtenerPorNodoAsync(int nodoId, CancellationToken cancellationToken = default)
        {
            return await _contexto.Nodos
                .AsNoTracking()
                .Where(nodo => nodo.Id == nodoId)
                .Select(nodo => nodo.Inversor)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task ActualizarAsync(InversorDomain inversor, CancellationToken cancellationToken = default)
        {
            var rastreado = BuscarRastreado(inversor.Id);

            if (rastreado is null)
            {
                _contexto.Inversores.Update(inversor);
            }
            else if (!ReferenceEquals(rastreado, inversor))
            {
                rastreado.ActualizarNombre(inversor.Nombre);
            }

            await _contexto.SaveChangesAsync(cancellationToken);
        }

        public async Task EliminarAsync(InversorDomain inversor, CancellationToken cancellationToken = default)
        {
            var rastreado = BuscarRastreado(inversor.Id);

            if (rastreado is null)
            {
                _contexto.Inversores.Remove(inversor);
            }
            else
            {
                _contexto.Inversores.Remove(rastreado);
            }

            await _contexto.SaveChangesAsync(cancellationToken);
        }

        private InversorDomain? BuscarRastreado(int inversorId)
        {
            return _contexto.Inversores.Local.FirstOrDefault(inversor => inversor.Id == inversorId);
        }
    }
}
