using Microsoft.AspNetCore.Mvc;
using Taller_.NET.Modulos.Historico.Application;
using Taller_.NET.Modulos.Historico.Application.Dtos;
using Taller_.NET.Modulos.Historico.Application.Excepciones;

namespace Taller_.NET.Modulos.Historico.Controllers
{
    [ApiController]
    [Route("api/historico-mantenibilidad")]
    public class HistoricoMantenibilidadController : ControllerBase
    {
        private readonly IHistoricoMantenibilidadService _historicoMantenibilidadService;

        public HistoricoMantenibilidadController(
            IHistoricoMantenibilidadService historicoMantenibilidadService)
        {
            _historicoMantenibilidadService = historicoMantenibilidadService;
        }

        [HttpGet("panel/{panelId:int}/nodo/{nodoId:int}/parque/{parqueSolarId:int}")]
        public async Task<IActionResult> ObtenerRegistrosPorPanel(
            int panelId,
            int nodoId,
            int parqueSolarId,
            CancellationToken cancellationToken)
        {
            try
            {
                var registros = await _historicoMantenibilidadService.ObtenerRegistrosPorPanelAsync(
                    panelId,
                    nodoId,
                    parqueSolarId,
                    cancellationToken);
                return Ok(registros);
            }
            catch (PanelFueraDeContextoException excepcion)
            {
                return NotFound(new { mensaje = excepcion.Message });
            }
            catch (UsuarioNoAutorizadoException)
            {
                return Forbid();
            }
        }

        [HttpGet("panel/{panelId:int}/nodo/{nodoId:int}/parque/{parqueSolarId:int}/contexto")]
        public async Task<IActionResult> ObtenerContextoPanel(
            int panelId,
            int nodoId,
            int parqueSolarId,
            CancellationToken cancellationToken)
        {
            try
            {
                var contexto = await _historicoMantenibilidadService.ObtenerContextoPanelAsync(
                    panelId,
                    nodoId,
                    parqueSolarId,
                    cancellationToken);
                return Ok(contexto);
            }
            catch (PanelFueraDeContextoException excepcion)
            {
                return NotFound(new { mensaje = excepcion.Message });
            }
        }

        [HttpGet("mantenedor")]
        public async Task<IActionResult> ObtenerMantenedorDeSesion(CancellationToken cancellationToken)
        {
            try
            {
                var mantenedor = await _historicoMantenibilidadService.ObtenerMantenedorDeSesionAsync(
                    cancellationToken);
                return Ok(mantenedor);
            }
            catch (MantenedorNoEncontradoException excepcion)
            {
                return NotFound(new { mensaje = excepcion.Message });
            }
            catch (UsuarioNoAutorizadoException)
            {
                return Forbid();
            }
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarRegistro(
            [FromBody] CrearHistoricoMantenibilidadDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                var id = await _historicoMantenibilidadService.RegistrarRegistroAsync(dto, cancellationToken);
                return Ok(id);
            }
            catch (PanelFueraDeContextoException excepcion)
            {
                return NotFound(new { mensaje = excepcion.Message });
            }
            catch (MantenedorNoEncontradoException excepcion)
            {
                return NotFound(new { mensaje = excepcion.Message });
            }
            catch (MantenedorSinRolException)
            {
                return Forbid();
            }
            catch (UsuarioNoAutorizadoException)
            {
                return Forbid();
            }
        }
    }
}
