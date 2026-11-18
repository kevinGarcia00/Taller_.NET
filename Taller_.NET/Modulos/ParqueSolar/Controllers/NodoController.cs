using Microsoft.AspNetCore.Mvc;
using Taller_.NET.Modulos.ParqueSolar.Application;
using Taller_.NET.Modulos.ParqueSolar.Application.Dtos;
using Taller_.NET.Modulos.ParqueSolar.Application.Excepciones;

namespace Taller_.NET.Modulos.ParqueSolar.Controllers
{
    [ApiController]
    [Route("api")]
    public class NodoController : ControllerBase
    {
        private readonly INodoService _nodoService;

        public NodoController(INodoService nodoService)
        {
            _nodoService = nodoService;
        }

        [HttpGet("parque/{parqueSolarId:int}/nodo")]
        public async Task<IActionResult> ObtenerNodosPorParque(
            int parqueSolarId,
            CancellationToken cancellationToken)
        {
            try
            {
                var nodos = await _nodoService.ObtenerNodosPorParqueAsync(parqueSolarId, cancellationToken);
                return Ok(nodos);
            }
            catch (ParqueNoEncontradoException excepcion)
            {
                return NotFound(new { mensaje = excepcion.Message });
            }
        }

        [HttpPost("nodo")]
        public async Task<IActionResult> CrearNodo(
            [FromBody] CrearNodoDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                var id = await _nodoService.CrearNodoAsync(dto, cancellationToken);
                return Ok(id);
            }
            catch (ParqueNoEncontradoException excepcion)
            {
                return NotFound(new { mensaje = excepcion.Message });
            }
            catch (NodoNombreDuplicadoException excepcion)
            {
                return Conflict(new { mensaje = excepcion.Message });
            }
        }

        [HttpGet("parque/{parqueSolarId:int}/nodo/{nodoId:int}")]
        public async Task<IActionResult> ObtenerNodoParaEdicion(
            int parqueSolarId,
            int nodoId,
            CancellationToken cancellationToken)
        {
            try
            {
                var nodo = await _nodoService.ObtenerNodoParaEdicionAsync(nodoId, parqueSolarId, cancellationToken);
                return Ok(nodo);
            }
            catch (NodoNoEncontradoException excepcion)
            {
                return NotFound(new { mensaje = excepcion.Message });
            }
        }

        [HttpGet("parque/{parqueSolarId:int}/nodo/{nodoId:int}/inversores")]
        public async Task<IActionResult> ObtenerInversoresParaEdicion(
            int parqueSolarId,
            int nodoId,
            CancellationToken cancellationToken)
        {
            try
            {
                var inversores = await _nodoService.ObtenerInversoresParaEdicionAsync(
                    nodoId,
                    parqueSolarId,
                    cancellationToken);
                return Ok(inversores);
            }
            catch (NodoNoEncontradoException excepcion)
            {
                return NotFound(new { mensaje = excepcion.Message });
            }
        }

        [HttpPut("nodo")]
        public async Task<IActionResult> ModificarNodo(
            [FromBody] EditarNodoDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                await _nodoService.ModificarNodoAsync(dto, cancellationToken);
                return NoContent();
            }
            catch (NodoNoEncontradoException excepcion)
            {
                return NotFound(new { mensaje = excepcion.Message });
            }
            catch (NodoNombreDuplicadoException excepcion)
            {
                return Conflict(new { mensaje = excepcion.Message });
            }
            catch (InversorNoEncontradoException excepcion)
            {
                return NotFound(new { mensaje = excepcion.Message });
            }
            catch (InversorYaAsignadoException excepcion)
            {
                return Conflict(new { mensaje = excepcion.Message });
            }
        }

        [HttpDelete("parque/{parqueSolarId:int}/nodo/{nodoId:int}")]
        public async Task<IActionResult> EliminarNodo(
            int parqueSolarId,
            int nodoId,
            CancellationToken cancellationToken)
        {
            try
            {
                await _nodoService.EliminarNodoAsync(nodoId, parqueSolarId, cancellationToken);
                return NoContent();
            }
            catch (NodoNoEncontradoException excepcion)
            {
                return NotFound(new { mensaje = excepcion.Message });
            }
        }
    }
}
