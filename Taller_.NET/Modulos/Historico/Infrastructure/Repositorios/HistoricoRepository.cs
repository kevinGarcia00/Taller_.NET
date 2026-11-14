using Microsoft.EntityFrameworkCore;
using Taller_.NET.Modulos.Historico.Domain;
using Taller_.NET.Modulos.Historico.Infrastructure.Persistencia;
using EntidadHistorico = Taller_.NET.Modulos.Historico.Domain.Historico;

namespace Taller_.NET.Modulos.Historico.Infrastructure.Repositorios
{
    public class HistoricoRepository : IHistoricoRepository
    {
        private readonly IHistoricoDbContext _contexto;

        public HistoricoRepository(IHistoricoDbContext contexto)
        {
            _contexto = contexto;
        }

        public async Task AgregarAsync(EntidadHistorico registro, CancellationToken cancellationToken = default)
        {
            await _contexto.Registros.AddAsync(registro, cancellationToken);
        }

        public async Task<bool> ExisteRegistroDelPanelEnPeriodoAsync(
            int panelId,
            DateTime periodoInicio,
            DateTime periodoFin,
            CancellationToken cancellationToken = default)
        {
            return await _contexto.Registros.AnyAsync(
                registro => registro.PanelId == panelId
                    && registro.Fecha >= periodoInicio
                    && registro.Fecha < periodoFin,
                cancellationToken);
        }

        public async Task<IReadOnlyList<EntidadHistorico>> ObtenerPorPanelAsync(
            int panelId,
            CancellationToken cancellationToken = default)
        {
            return await _contexto.Registros
                .Where(registro => registro.PanelId == panelId)
                .OrderByDescending(registro => registro.Fecha)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<EntidadHistorico>> ObtenerPorPanelYFechaAsync(
            int panelId,
            DateTime fecha,
            CancellationToken cancellationToken = default)
        {
            // La fecha la elige el usuario en hora local, mientras que los registros
            // se persisten en UTC: se consulta el dia local completo.
            var inicio = fecha.Date.ToUniversalTime();
            var fin = fecha.Date.AddDays(1).ToUniversalTime();

            return await _contexto.Registros
                .Where(registro => registro.PanelId == panelId
                    && registro.Fecha >= inicio
                    && registro.Fecha < fin)
                .OrderByDescending(registro => registro.Fecha)
                .ToListAsync(cancellationToken);
        }

        public async Task GuardarCambiosAsync(CancellationToken cancellationToken = default)
        {
            await _contexto.SaveChangesAsync(cancellationToken);
        }
    }
}
