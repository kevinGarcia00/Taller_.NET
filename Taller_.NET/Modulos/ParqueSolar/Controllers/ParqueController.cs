using Microsoft.AspNetCore.Mvc;
using Taller_.NET.Modulos.ParqueSolar.Application;
using Taller_.NET.Modulos.ParqueSolar.Application.Dtos;
using Taller_.NET.Modulos.ParqueSolar.Application.Excepciones;

namespace Taller_.NET.Modulos.ParqueSolar.Controllers
{
    [ApiController]
    [Route("api/parque")]
    public class ParqueController : ControllerBase
    {
        private readonly IParqueService _parqueService;

        public ParqueController(IParqueService parqueService)
        {
            _parqueService = parqueService;
        }

        [HttpGet]
        public IActionResult ObtenerParques()
        {
            return Ok(_parqueService.ObtenerParquesSync());
        }

        [HttpGet("{parqueSolarId:int}")]
        public IActionResult ObtenerParque(int parqueSolarId)
        {
            try
            {
                return Ok(_parqueService.ObtenerParqueSync(parqueSolarId));
            }
            catch (ParqueNoEncontradoException excepcion)
            {
                return NotFound(new { mensaje = excepcion.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CrearParque(
            [FromBody] CrearParqueDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                var id = await _parqueService.CrearParqueAsync(dto, cancellationToken);
                return Ok(id);
            }
            catch (ParqueNombreDuplicadoException excepcion)
            {
                return Conflict(new { mensaje = excepcion.Message });
            }
        }

        [HttpPut("{parqueSolarId:int}")]
        public async Task<IActionResult> ModificarParque(
            int parqueSolarId,
            [FromBody] ModificarParqueDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                var id = await _parqueService.ModificarParqueAsync(parqueSolarId, dto, cancellationToken);
                return Ok(id);
            }
            catch (ParqueNoEncontradoException excepcion)
            {
                return NotFound(new { mensaje = excepcion.Message });
            }
            catch (ParqueNombreDuplicadoException excepcion)
            {
                return Conflict(new { mensaje = excepcion.Message });
            }
        }

        [HttpDelete("{parqueSolarId:int}")]
        public async Task<IActionResult> EliminarParque(
            int parqueSolarId,
            CancellationToken cancellationToken)
        {
            try
            {
                await _parqueService.EliminarParqueAsync(parqueSolarId, cancellationToken);
                return NoContent();
            }
            catch (ParqueNoEncontradoException excepcion)
            {
                return NotFound(new { mensaje = excepcion.Message });
            }
            catch (ParqueConNodosAsociadosException excepcion)
            {
                return Conflict(new { mensaje = excepcion.Message });
            }
        }
    }
}
