namespace Application.Dtos;

public sealed class UsuarioListadoDto
{
    public Guid Id { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public string Apellido { get; init; } = string.Empty;
    public string Correo { get; init; } = string.Empty;
    public string Telefono { get; init; } = string.Empty;
    public string? Auth0UserId { get; init; }
}
