using Microsoft.AspNetCore.Mvc;
using Taller_.NET.Modulos.ParqueSolar.Application;
using Taller_.NET.Modulos.ParqueSolar.Application.Dtos;
using Taller_.NET.Modulos.ParqueSolar.Application.Excepciones;

namespace Taller_.NET.Modulos.ParqueSolar.Controllers
{
    [ApiController]
    [Route("api")]
    public class InversorController : ControllerBase
    {
        private readonly IInversorService _inversorService;

        public InversorController(IInversorService inversorService)
        {
            _inversorService = inversorService;
        }

        [HttpGet("parque/{parqueSolarId:int}/nodo/{nodoId:int}/inversor")]
        public async Task<IActionResult> ObtenerInversorDelNodo(
            int parqueSolarId,
            int nodoId,
            CancellationToken cancellationToken)
        {
            try
            {
                var inversor = await _inversorService.ObtenerInversorDelNodoAsync(
                    nodoId,
                    parqueSolarId,
                    cancellationToken);

                return inversor is null
                    ? NotFound(new { mensaje = $"El nodo {nodoId} no tiene un inversor asignado." })
                    : Ok(inversor);
            }
            catch (NodoNoEncontradoException excepcion)
            {
                return NotFound(new { mensaje = excepcion.Message });
            }
        }

        [HttpPost("inversor")]
        public async Task<IActionResult> CrearInversor(
            [FromBody] CrearInversorDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                var id = await _inversorService.CrearInversorAsync(dto, cancellationToken);
                return Ok(id);
            }
            catch (NodoNoEncontradoException excepcion)
            {
                return NotFound(new { mensaje = excepcion.Message });
            }
            catch (NodoYaTieneInversorException excepcion)
            {
                return Conflict(new { mensaje = excepcion.Message });
            }
        }

        [HttpGet("parque/{parqueSolarId:int}/nodo/{nodoId:int}/inversor/edicion")]
        public async Task<IActionResult> ObtenerInversorParaEdicion(
            int parqueSolarId,
            int nodoId,
            CancellationToken cancellationToken)
        {
            try
            {
                var inversor = await _inversorService.ObtenerParaEdicionAsync(
                    nodoId,
                    parqueSolarId,
                    cancellationToken);
                return Ok(inversor);
            }
            catch (NodoNoEncontradoException excepcion)
            {
                return NotFound(new { mensaje = excepcion.Message });
            }
            catch (InversorNoEncontradoException excepcion)
            {
                return NotFound(new { mensaje = excepcion.Message });
            }
        }

        [HttpPut("inversor")]
        public async Task<IActionResult> ModificarInversor(
            [FromBody] ModificarInversorDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                await _inversorService.ModificarInversorAsync(dto, cancellationToken);
                return NoContent();
            }
            catch (NodoNoEncontradoException excepcion)
            {
                return NotFound(new { mensaje = excepcion.Message });
            }
            catch (InversorNoEncontradoException excepcion)
            {
                return NotFound(new { mensaje = excepcion.Message });
            }
        }

        [HttpDelete("parque/{parqueSolarId:int}/nodo/{nodoId:int}/inversor")]
        public async Task<IActionResult> EliminarInversor(
            int parqueSolarId,
            int nodoId,
            CancellationToken cancellationToken)
        {
            try
            {
                await _inversorService.EliminarInversorAsync(nodoId, parqueSolarId, cancellationToken);
                return NoContent();
            }
            catch (NodoNoEncontradoException excepcion)
            {
                return NotFound(new { mensaje = excepcion.Message });
            }
            catch (InversorNoEncontradoException excepcion)
            {
                return NotFound(new { mensaje = excepcion.Message });
            }
        }
    }
}
