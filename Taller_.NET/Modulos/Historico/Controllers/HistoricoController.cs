using Microsoft.AspNetCore.Mvc;
using Taller_.NET.Modulos.Historico.Application;
using Taller_.NET.Modulos.Historico.Application.Dtos;
using Taller_.NET.Modulos.Historico.Application.Excepciones;

namespace Taller_.NET.Modulos.Historico.Controllers
{
    [ApiController]
    [Route("api/historico")]
    public class HistoricoController : ControllerBase
    {
        private readonly IHistoricoService _historicoService;

        public HistoricoController(IHistoricoService historicoService)
        {
            _historicoService = historicoService;
        }

        [HttpGet("panel/{panelId:int}")]
        public async Task<IActionResult> ObtenerRegistrosPorPanel(
            int panelId,
            CancellationToken cancellationToken)
        {
            var registros = await _historicoService.ObtenerRegistrosPorPanelAsync(panelId, cancellationToken);
            return Ok(registros);
        }

        [HttpGet("panel/{panelId:int}/nodo/{nodoId:int}/parque/{parqueSolarId:int}")]
        public async Task<IActionResult> ObtenerRegistrosFiltradosPorPanel(
            int panelId,
            int nodoId,
            int parqueSolarId,
            [FromQuery] FiltroHistoricoDto filtro,
            CancellationToken cancellationToken)
        {
            try
            {
                var registros = await _historicoService.ObtenerRegistrosFiltradosPorPanelAsync(
                    panelId,
                    nodoId,
                    parqueSolarId,
                    filtro,
                    cancellationToken);
                return Ok(registros);
            }
            catch (PanelNoEncontradoException excepcion)
            {
                return NotFound(new { mensaje = excepcion.Message });
            }
        }

        [HttpPost("generar-registros")]
        public async Task<IActionResult> GenerarRegistrosDelPeriodo(
            [FromQuery] TimeSpan intervalo,
            CancellationToken cancellationToken)
        {
            var cantidad = await _historicoService.GenerarRegistrosDelPeriodoAsync(intervalo, cancellationToken);
            return Ok(cantidad);
        }
    }
}
