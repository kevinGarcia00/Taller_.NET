using System.ComponentModel;
using System.Text.Json;
using ModelContextProtocol.Server;
using Application.Puertos;

namespace Presentacion.Herramientas;

[McpServerToolType]
public sealed class HerramientaUsuario
{
    private static readonly JsonSerializerOptions OpcionesJson = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    private readonly IUsuarioConsultaService _usuarioConsultaService;

    public HerramientaUsuario(IUsuarioConsultaService usuarioConsultaService)
    {
        _usuarioConsultaService = usuarioConsultaService;
    }

    [McpServerTool(Name = "listar_usuarios"), Description("Responde '¿Qué usuarios hay registrados?'. Devuelve id, nombre, apellido, correo, teléfono e identificador de Auth0 de cada usuario.")]
    public async Task<string> ListarUsuarios()
    {
        return Serializar(await _usuarioConsultaService.ObtenerUsuariosAsync());
    }

    private static string Serializar(object resultado)
    {
        return JsonSerializer.Serialize(resultado, OpcionesJson);
    }
}
