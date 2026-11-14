using FluentValidation;
using Taller_.NET.Modulos.Historico.Application.Dtos;
using Taller_.NET.Modulos.Historico.Application.Excepciones;
using Taller_.NET.Modulos.Historico.Application.Puertos;
using Taller_.NET.Modulos.Historico.Domain;
using EntidadHistorico = Taller_.NET.Modulos.Historico.Domain.Historico;

namespace Taller_.NET.Modulos.Historico.Application.Servicios
{
    /// <summary>
    /// Genera un registro de historico por cada panel, tomando la energia total
    /// desde ParqueSolar a traves del mediador y evitando duplicados dentro del
    /// mismo periodo (definido por el intervalo configurado).
    /// Tambien expone la lectura de los registros de un panel, con o sin filtro
    /// por fecha.
    /// </summary>
    public class HistoricoService : IHistoricoService
    {
        private readonly IHistoricoRepository _historicoRepository;
        private readonly IMediadorHistoricoParqueSolar _mediadorHistoricoParqueSolar;
        private readonly IValidator<FiltroHistoricoDto> _validadorFiltro;
        private readonly ILogger<HistoricoService> _logger;

        public HistoricoService(
            IHistoricoRepository historicoRepository,
            IMediadorHistoricoParqueSolar mediadorHistoricoParqueSolar,
            IValidator<FiltroHistoricoDto> validadorFiltro,
            ILogger<HistoricoService> logger)
        {
            _historicoRepository = historicoRepository;
            _mediadorHistoricoParqueSolar = mediadorHistoricoParqueSolar;
            _validadorFiltro = validadorFiltro;
            _logger = logger;
        }

        public async Task<int> GenerarRegistrosDelPeriodoAsync(
            TimeSpan intervalo,
            CancellationToken cancellationToken = default)
        {
            var paneles = await _mediadorHistoricoParqueSolar.ObtenerEnergiaTotalPanelesAsync(cancellationToken);

            if (paneles.Count == 0)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("No hay paneles registrados: no se generan registros de historico.");
                }

                return 0;
            }

            var fecha = DateTime.UtcNow;
            var (periodoInicio, periodoFin) = CalcularPeriodo(fecha, intervalo);
            var cantidadCreada = 0;

            foreach (var panel in paneles)
            {
                var yaExiste = await _historicoRepository.ExisteRegistroDelPanelEnPeriodoAsync(
                    panel.PanelId,
                    periodoInicio,
                    periodoFin,
                    cancellationToken);

                if (yaExiste)
                {
                    continue;
                }

                var contextoPanel = new ContextoPanel(panel.PanelId, panel.NodoId, panel.ParqueSolarId);
                var registro = new EntidadHistorico(fecha, panel.EnergiaTotal, contextoPanel);

                await _historicoRepository.AgregarAsync(registro, cancellationToken);
                cantidadCreada++;
            }

            if (cantidadCreada > 0)
            {
                await _historicoRepository.GuardarCambiosAsync(cancellationToken);
            }

            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation(
                    "Generacion de historico finalizada: {Cantidad} registros creados.",
                    cantidadCreada);
            }

            return cantidadCreada;
        }

        public async Task<IReadOnlyList<HistoricoListadoDto>> ObtenerRegistrosPorPanelAsync(
            int panelId,
            CancellationToken cancellationToken = default)
        {
            var registros = await _historicoRepository.ObtenerPorPanelAsync(panelId, cancellationToken);

            return Proyectar(registros);
        }

        public async Task<IReadOnlyList<HistoricoListadoDto>> ObtenerRegistrosFiltradosPorPanelAsync(
            int panelId,
            int nodoId,
            int parqueSolarId,
            FiltroHistoricoDto filtro,
            CancellationToken cancellationToken = default)
        {
            await ValidarFiltroAsync(filtro, cancellationToken);

            var panelExiste = await _mediadorHistoricoParqueSolar.ExistePanelAsync(
                panelId,
                nodoId,
                parqueSolarId,
                cancellationToken);

            if (!panelExiste)
            {
                throw new PanelNoEncontradoException(panelId);
            }

            var registros = await _historicoRepository.ObtenerPorPanelYFechaAsync(
                panelId,
                filtro.Referencia,
                cancellationToken);

            return Proyectar(registros);
        }

        private async Task ValidarFiltroAsync(FiltroHistoricoDto filtro, CancellationToken cancellationToken)
        {
            var resultado = await _validadorFiltro.ValidateAsync(filtro, cancellationToken);

            if (!resultado.IsValid)
            {
                throw new ValidationException(resultado.Errors);
            }
        }

        private static List<HistoricoListadoDto> Proyectar(IReadOnlyList<EntidadHistorico> registros)
        {
            return registros
                .Select(registro => new HistoricoListadoDto
                {
                    Id = registro.Id,
                    Fecha = registro.Fecha,
                    EnergiaTotal = registro.EnergiaTotal
                })
                .ToList();
        }

        private static (DateTime Inicio, DateTime Fin) CalcularPeriodo(DateTime momentoUtc, TimeSpan intervalo)
        {
            var momentoLocal = momentoUtc.ToLocalTime();
            var inicioLocal = new DateTime(
                momentoLocal.Ticks - (momentoLocal.Ticks % intervalo.Ticks),
                DateTimeKind.Local);

            var inicio = inicioLocal.ToUniversalTime();
            return (inicio, inicio + intervalo);
        }
    }
}
