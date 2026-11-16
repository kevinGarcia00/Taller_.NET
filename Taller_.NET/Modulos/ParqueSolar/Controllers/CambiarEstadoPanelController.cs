using Microsoft.AspNetCore.Mvc;
using Taller_.NET.Modulos.ParqueSolar.Application;
using Taller_.NET.Modulos.ParqueSolar.Application.Dtos;
using Taller_.NET.Modulos.ParqueSolar.Application.Excepciones;
using Taller_.NET.Modulos.ParqueSolar.Domain.Excepciones;

namespace Taller_.NET.Modulos.ParqueSolar.Controllers
{
    [ApiController]
    [Route("api/inversor")]
    public class CambiarEstadoPanelController : ControllerBase
    {
        private readonly ICambiarEstadoPanelService _cambiarEstadoPanelService;

        public CambiarEstadoPanelController(ICambiarEstadoPanelService cambiarEstadoPanelService)
        {
            _cambiarEstadoPanelService = cambiarEstadoPanelService;
        }

        [HttpPut("{idInversor:int}/nodo/{idNodo:int}/panel/{idPanel:int}/estado")]
        public async Task<IActionResult> CambiarEstado(
            int idInversor,
            int idNodo,
            int idPanel,
            [FromBody] CambiarEstadoPanelDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                var panel = await _cambiarEstadoPanelService.CambiarEstadoPanelAsync(
                    idInversor,
                    idNodo,
                    idPanel,
                    dto,
                    cancellationToken);

                return Ok(panel);
            }
            catch (PanelSinCambiosException excepcion)
            {
                return Conflict(new { mensaje = excepcion.Message });
            }
            catch (InversorNoEncontradoException excepcion)
            {
                return NotFound(new { mensaje = excepcion.Message });
            }
            catch (NodoNoEncontradoException excepcion)
            {
                return NotFound(new { mensaje = excepcion.Message });
            }
            catch (PanelNoEncontradoException excepcion)
            {
                return NotFound(new { mensaje = excepcion.Message });
            }
            catch (ArgumentOutOfRangeException excepcion)
            {
                return BadRequest(new { mensaje = excepcion.Message });
            }
            catch (PanelInvalidoException excepcion)
            {
                return BadRequest(new { mensaje = excepcion.Message });
            }
        }
    }
}
