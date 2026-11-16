using Microsoft.AspNetCore.Mvc;
using Taller_.NET.Modulos.ParqueSolar.Application;
using Taller_.NET.Modulos.ParqueSolar.Application.Dtos;
using Taller_.NET.Modulos.ParqueSolar.Application.Excepciones;
using Taller_.NET.Modulos.ParqueSolar.Domain.Excepciones;

namespace Taller_.NET.Modulos.ParqueSolar.Controllers
{
    [ApiController]
    [Route("api/inversor")]
    public class CambiarVolcadoEnergiaPanelController : ControllerBase
    {
        private readonly ICambiarVolcadoEnergiaPanelService _cambiarVolcadoEnergiaPanelService;

        public CambiarVolcadoEnergiaPanelController(
            ICambiarVolcadoEnergiaPanelService cambiarVolcadoEnergiaPanelService)
        {
            _cambiarVolcadoEnergiaPanelService = cambiarVolcadoEnergiaPanelService;
        }

        [HttpPut("{idInversor:int}/nodo/{idNodo:int}/panel/{idPanel:int}/volcado")]
        public async Task<IActionResult> CambiarVolcadoEnergia(
            int idInversor,
            int idNodo,
            int idPanel,
            [FromBody] CambiarVolcadoEnergiaPanelDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                var panel = await _cambiarVolcadoEnergiaPanelService.CambiarVolcadoEnergiaPanelAsync(
                    idInversor,
                    idNodo,
                    idPanel,
                    dto,
                    cancellationToken);

                return Ok(panel);
            }
            catch (VolcadoEnergiaSinCambiosException excepcion)
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
