using Microsoft.AspNetCore.Mvc;

namespace Taller_.NET.Modulos.Auth.Controllers;

[ApiController]
[Route("api/webhooks")]
public class Auth0WebhookController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<Auth0WebhookController> _logger;

    public Auth0WebhookController(IConfiguration configuration, ILogger<Auth0WebhookController> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    [HttpPost("post-user-registration")]
    public IActionResult PostUserRegistration(
        [FromBody] Auth0UserRegisteredDto payload,
        [FromHeader(Name = "Authorization")] string? authorization)
    {
        var expectedSecret = _configuration["Auth0Webhook:Secret"];

        if (string.IsNullOrEmpty(authorization))
        {
            _logger.LogWarning("No llega el header Authorization");
            return Unauthorized(new { message = "Token faltante" });
        }

        var expectedToken = $"Bearer {expectedSecret}";
        if (authorization != expectedToken)
        {
            _logger.LogWarning("Token inválido: {Received} | Esperado: {Expected}", authorization, expectedToken);
            return Unauthorized(new { message = "Token inválido" });
        }

        _logger.LogInformation("Nuevo usuario registrado");
        return Ok(new { success = true, message = "Usuario sincronizado correctamente" });
    }
}

public class Auth0UserRegisteredDto
{
    public required string UserId { get; set; }
    public required string Email { get; set; }
    public required string Username { get; set; }
}