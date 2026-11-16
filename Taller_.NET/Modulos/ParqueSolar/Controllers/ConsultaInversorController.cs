using Microsoft.AspNetCore.Mvc;
using Taller_.NET.Modulos.ParqueSolar.Application;
using Taller_.NET.Modulos.ParqueSolar.Application.Excepciones;

namespace Taller_.NET.Modulos.ParqueSolar.Controllers
{
    [ApiController]
    [Route("api/inversor")]
    public class ConsultaInversorController : ControllerBase
    {
        private readonly IConsultaInversorService _consultaInversorService;

        public ConsultaInversorController(IConsultaInversorService consultaInversorService)
        {
            _consultaInversorService = consultaInversorService;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerInversores(CancellationToken cancellationToken)
        {
            var inversores = await _consultaInversorService.ObtenerInversoresAsync(cancellationToken);

            return Ok(inversores);
        }

        [HttpGet("{idInversor:int}")]
        public async Task<IActionResult> ObtenerInversor(
            int idInversor,
            CancellationToken cancellationToken)
        {
            try
            {
                var inversor = await _consultaInversorService.ObtenerInversorAsync(
                    idInversor,
                    cancellationToken);

                return inversor is null
                    ? NotFound(new { mensaje = $"No existe un inversor con el identificador '{idInversor}'." })
                    : Ok(inversor);
            }
            catch (ArgumentOutOfRangeException excepcion)
            {
                return BadRequest(new { mensaje = excepcion.Message });
            }
        }

        [HttpGet("{idInversor:int}/nodo")]
        public async Task<IActionResult> ObtenerNodos(
            int idInversor,
            CancellationToken cancellationToken)
        {
            try
            {
                var nodos = await _consultaInversorService.ObtenerNodosInversorAsync(
                    idInversor,
                    cancellationToken);

                return Ok(nodos);
            }
            catch (InversorNoEncontradoException excepcion)
            {
                return NotFound(new { mensaje = excepcion.Message });
            }
            catch (ArgumentOutOfRangeException excepcion)
            {
                return BadRequest(new { mensaje = excepcion.Message });
            }
        }

        [HttpGet("{idInversor:int}/nodo/{idNodo:int}/panel")]
        public async Task<IActionResult> ObtenerPaneles(
            int idInversor,
            int idNodo,
            CancellationToken cancellationToken)
        {
            try
            {
                var paneles = await _consultaInversorService.ObtenerPanelesNodoInversorAsync(
                    idInversor,
                    idNodo,
                    cancellationToken);

                return Ok(paneles);
            }
            catch (InversorNoEncontradoException excepcion)
            {
                return NotFound(new { mensaje = excepcion.Message });
            }
            catch (NodoNoEncontradoException excepcion)
            {
                return NotFound(new { mensaje = excepcion.Message });
            }
            catch (ArgumentOutOfRangeException excepcion)
            {
                return BadRequest(new { mensaje = excepcion.Message });
            }
        }

        [HttpGet("{idInversor:int}/nodo/{idNodo:int}/panel/{idPanel:int}")]
        public async Task<IActionResult> ObtenerPanel(
            int idInversor,
            int idNodo,
            int idPanel,
            CancellationToken cancellationToken)
        {
            try
            {
                var panel = await _consultaInversorService.ObtenerPanelInversorAsync(
                    idInversor,
                    idNodo,
                    idPanel,
                    cancellationToken);

                return panel is null
                    ? NotFound(new { mensaje = $"No se encontro el panel {idPanel} en el nodo {idNodo}." })
                    : Ok(panel);
            }
            catch (InversorNoEncontradoException excepcion)
            {
                return NotFound(new { mensaje = excepcion.Message });
            }
            catch (NodoNoEncontradoException excepcion)
            {
                return NotFound(new { mensaje = excepcion.Message });
            }
            catch (ArgumentOutOfRangeException excepcion)
            {
                return BadRequest(new { mensaje = excepcion.Message });
            }
        }
    }
}
