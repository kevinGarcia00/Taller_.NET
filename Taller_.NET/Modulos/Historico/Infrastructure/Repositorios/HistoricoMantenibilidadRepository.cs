using Microsoft.EntityFrameworkCore;
using Taller_.NET.Modulos.Historico.Domain;
using Taller_.NET.Modulos.Historico.Infrastructure.Persistencia;

namespace Taller_.NET.Modulos.Historico.Infrastructure.Repositorios
{
    public class HistoricoMantenibilidadRepository : IHistoricoMantenibilidadRepository
    {
        private readonly IHistoricoDbContext _contexto;

        public HistoricoMantenibilidadRepository(IHistoricoDbContext contexto)
        {
            _contexto = contexto;
        }

        public async Task<int> AgregarAsync(
            HistoricoMantenibilidad registro,
            CancellationToken cancellationToken = default)
        {
            await _contexto.Registros.AddAsync(registro, cancellationToken);
            await _contexto.SaveChangesAsync(cancellationToken);

            return registro.Id;
        }

        public async Task<IReadOnlyList<HistoricoMantenibilidad>> ObtenerPorPanelAsync(
            int panelId,
            CancellationToken cancellationToken = default)
        {
            return await _contexto.Registros
                .AsNoTracking()
                .Where(registro => registro.ContextoPanel.PanelId == panelId)
                .OrderByDescending(registro => registro.Fecha)
                .ToListAsync(cancellationToken);
        }
    }
}
