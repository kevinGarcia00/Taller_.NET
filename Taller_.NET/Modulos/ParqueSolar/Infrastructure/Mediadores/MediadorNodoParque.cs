using Microsoft.EntityFrameworkCore;
using Taller_.NET.Modulos.ParqueSolar.Application.Puertos;
using Taller_.NET.Modulos.ParqueSolar.Infrastructure.Persistencia;

namespace Taller_.NET.Modulos.ParqueSolar.Infrastructure.Mediadores
{
    public class MediadorNodoParque : IMediadorNodoParque
    {
        private readonly IParqueDbContext _contexto;

        public MediadorNodoParque(IParqueDbContext contexto)
        {
            _contexto = contexto;
        }

        public async Task<bool> ExisteParqueAsync(
            int parqueSolarId,
            CancellationToken cancellationToken = default)
        {
            return await _contexto.Parques
                .AnyAsync(parque => parque.Id == parqueSolarId, cancellationToken);
        }
    }
}
