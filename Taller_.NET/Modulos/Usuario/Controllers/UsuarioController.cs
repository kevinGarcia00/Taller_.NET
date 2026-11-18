using Microsoft.AspNetCore.Mvc;
using Taller_.NET.Modulos.Usuario.Application;
using Taller_.NET.Modulos.Usuario.Application.Excepciones;

namespace Taller_.NET.Modulos.Usuario.Controllers
{
    [ApiController]
    [Route("api/usuario")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerUsuarios(CancellationToken cancellationToken)
        {
            try
            {
                var usuarios = await _usuarioService.ObtenerUsuariosAsync(cancellationToken);
                return Ok(usuarios);
            }
            catch (UsuarioNoAutorizadoException)
            {
                return Forbid();
            }
        }

        [HttpGet("roles")]
        public async Task<IActionResult> ObtenerRoles(CancellationToken cancellationToken)
        {
            try
            {
                var roles = await _usuarioService.ObtenerRolesAsync(cancellationToken);
                return Ok(roles);
            }
            catch (UsuarioNoAutorizadoException)
            {
                return Forbid();
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> ObtenerParaEdicion(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var usuario = await _usuarioService.ObtenerParaEdicionAsync(id, cancellationToken);
                return Ok(usuario);
            }
            catch (UsuarioNoEncontradoException excepcion)
            {
                return NotFound(new { mensaje = excepcion.Message });
            }
            catch (UsuarioNoAutorizadoException)
            {
                return Forbid();
            }
        }
    }
}
