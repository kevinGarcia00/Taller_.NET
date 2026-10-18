using Microsoft.AspNetCore.Mvc;

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
    public IActionResult PostUserRegistration([FromBody] Auth0UserRegisteredDto payload)
    {
        var expectedSecret = _configuration["Auth0Webhook:Secret"];
    
        if (!Request.Headers.TryGetValue("Authorization", out var authHeader))
        {
            _logger.LogWarning("No llega el header Authorization");
            return Unauthorized(new { message = "Token faltante" });
        }

        var expectedToken = $"Bearer {expectedSecret}";
        if (authHeader != expectedToken)
        {
            _logger.LogWarning("Token inválido: {Received} | Esperado: {Expected}", authHeader.ToString(), expectedToken);
            return Unauthorized(new { message = "Token inválido" });
        }

        _logger.LogInformation("Nuevo usuario registrado");
        return Ok(new { success = true, message = "Usuario sincronizado correctamente" });
    }
}

public class Auth0UserRegisteredDto
{
    public string UserId { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
}