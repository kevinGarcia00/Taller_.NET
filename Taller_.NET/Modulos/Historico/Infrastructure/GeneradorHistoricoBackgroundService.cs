using Taller_.NET.Modulos.Historico.Application;

namespace Taller_.NET.Modulos.Historico.Infrastructure
{
    /// <summary>
    /// Proceso programado que dispara el alta automatica de los registros de
    /// historico segun el intervalo configurado con "Historico:IntervaloHoras".
    /// </summary>
    public class GeneradorHistoricoBackgroundService : BackgroundService
    {
        private const string ClaveIntervaloHoras = "Historico:IntervaloHoras";
        
        private const double IntervaloPorDefectoEnHoras = 24;

        // Prueba para testear que genere el intervalo, en produccion quedara con 24 horas
        //private const double IntervaloPorDefectoEnHoras = 5.0 / 60.0;

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<GeneradorHistoricoBackgroundService> _logger;
        private readonly TimeSpan _intervalo;

        public GeneradorHistoricoBackgroundService(
            IServiceScopeFactory scopeFactory,
            IConfiguration configuracion,
            ILogger<GeneradorHistoricoBackgroundService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;

            var horas = configuracion.GetValue<double?>(ClaveIntervaloHoras) ?? IntervaloPorDefectoEnHoras;
            _intervalo = TimeSpan.FromHours(horas);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var temporizador = new PeriodicTimer(_intervalo);

            while (!stoppingToken.IsCancellationRequested)
            {
                await EjecutarGeneracionAsync(stoppingToken);

                try
                {
                    if (!await temporizador.WaitForNextTickAsync(stoppingToken))
                    {
                        break;
                    }
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }

        private async Task EjecutarGeneracionAsync(CancellationToken cancellationToken)
        {
            try
            {
                await using var alcance = _scopeFactory.CreateAsyncScope();
                var servicio = alcance.ServiceProvider.GetRequiredService<IHistoricoService>();

                var cantidadCreada = await servicio.GenerarRegistrosDelPeriodoAsync(_intervalo, cancellationToken);

                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation(
                        "El generador de historico creo {Cantidad} registros.",
                        cantidadCreada);
                }
            }
            catch (Exception excepcion)
            {
                _logger.LogError(excepcion, "Error al generar los registros de historico.");
            }
        }
    }
}
