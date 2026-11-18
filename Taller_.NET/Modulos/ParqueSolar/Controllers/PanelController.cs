using Microsoft.AspNetCore.Mvc;
using Taller_.NET.Modulos.ParqueSolar.Application;
using Taller_.NET.Modulos.ParqueSolar.Application.Dtos;
using Taller_.NET.Modulos.ParqueSolar.Application.Excepciones;

namespace Taller_.NET.Modulos.ParqueSolar.Controllers
{
    [ApiController]
    [Route("api")]
    public class PanelController : ControllerBase
    {
        private readonly IPanelService _panelService;

        public PanelController(IPanelService panelService)
        {
            _panelService = panelService;
        }

        [HttpGet("parque/{parqueSolarId:int}/nodo/{nodoId:int}/panel")]
        public async Task<IActionResult> ObtenerPanelesPorNodo(
            int parqueSolarId,
            int nodoId,
            CancellationToken cancellationToken)
        {
            try
            {
                var paneles = await _panelService.ObtenerPanelesPorNodoAsync(
                    nodoId,
                    parqueSolarId,
                    cancellationToken);
                return Ok(paneles);
            }
            catch (NodoNoEncontradoException excepcion)
            {
                return NotFound(new { mensaje = excepcion.Message });
            }
        }

        [HttpPost("panel")]
        public async Task<IActionResult> CrearPanel(
            [FromBody] CrearPanelDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                var id = await _panelService.CrearPanelAsync(dto, cancellationToken);
                return Ok(id);
            }
            catch (NodoNoEncontradoException excepcion)
            {
                return NotFound(new { mensaje = excepcion.Message });
            }
            catch (PanelNombreDuplicadoException excepcion)
            {
                return Conflict(new { mensaje = excepcion.Message });
            }
        }

        [HttpGet("parque/{parqueSolarId:int}/nodo/{nodoId:int}/panel/{panelId:int}")]
        public async Task<IActionResult> ObtenerPanel(
            int parqueSolarId,
            int nodoId,
            int panelId,
            CancellationToken cancellationToken)
        {
            try
            {
                var panel = await _panelService.ObtenerPanelAsync(
                    panelId,
                    nodoId,
                    parqueSolarId,
                    cancellationToken);
                return Ok(panel);
            }
            catch (NodoNoEncontradoException excepcion)
            {
                return NotFound(new { mensaje = excepcion.Message });
            }
            catch (PanelNoEncontradoException excepcion)
            {
                return NotFound(new { mensaje = excepcion.Message });
            }
        }

        [HttpGet("parque/{parqueSolarId:int}/nodo/{nodoId:int}/panel/{panelId:int}/edicion")]
        public async Task<IActionResult> ObtenerPanelParaEdicion(
            int parqueSolarId,
            int nodoId,
            int panelId,
            CancellationToken cancellationToken)
        {
            try
            {
                var panel = await _panelService.ObtenerParaEdicionAsync(
                    panelId,
                    nodoId,
                    cancellationToken);
                return Ok(panel);
            }
            catch (NodoNoEncontradoException excepcion)
            {
                return NotFound(new { mensaje = excepcion.Message });
            }
            catch (PanelNoEncontradoException excepcion)
            {
                return NotFound(new { mensaje = excepcion.Message });
            }
        }

        [HttpPut("panel")]
        public async Task<IActionResult> ModificarPanel(
            [FromBody] ModificarPanelDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                await _panelService.ModificarPanelAsync(dto, cancellationToken);
                return NoContent();
            }
            catch (NodoNoEncontradoException excepcion)
            {
                return NotFound(new { mensaje = excepcion.Message });
            }
            catch (PanelNoEncontradoException excepcion)
            {
                return NotFound(new { mensaje = excepcion.Message });
            }
            catch (PanelNombreDuplicadoException excepcion)
            {
                return Conflict(new { mensaje = excepcion.Message });
            }
        }

        [HttpDelete("parque/{parqueSolarId:int}/nodo/{nodoId:int}/panel/{panelId:int}")]
        public async Task<IActionResult> EliminarPanel(
            int parqueSolarId,
            int nodoId,
            int panelId,
            CancellationToken cancellationToken)
        {
            try
            {
                await _panelService.EliminarPanelAsync(panelId, nodoId, parqueSolarId, cancellationToken);
                return NoContent();
            }
            catch (NodoNoEncontradoException excepcion)
            {
                return NotFound(new { mensaje = excepcion.Message });
            }
            catch (PanelNoEncontradoException excepcion)
            {
                return NotFound(new { mensaje = excepcion.Message });
            }
        }
    }
}
