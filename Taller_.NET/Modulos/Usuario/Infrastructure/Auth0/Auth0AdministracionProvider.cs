using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Taller_.NET.Modulos.Usuario.Application.Dtos;
using Taller_.NET.Modulos.Usuario.Application.Excepciones;
using Taller_.NET.Modulos.Usuario.Application.Puertos;

namespace Taller_.NET.Modulos.Usuario.Infrastructure.Auth0
{
    /// <summary>
    /// Cliente de la Management API de Auth0. Auth0 es la fuente unica de
    /// identidad, roles y permisos; la base local solo conserva datos de negocio.
    /// </summary>
    public class Auth0AdministracionProvider : IAuth0AdministracionProvider
    {
        private const string ConexionBaseDeDatos = "Username-Password-Authentication";
        private const string RutaUsuarios = "api/v2/users";
        private const string RutaRoles = "api/v2/roles";
        private const int CantidadMaximaPorPagina = 100;

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<Auth0AdministracionProvider> _logger;

        private string? _tokenDeAcceso;
        private DateTimeOffset _expiracionDelToken;

        public Auth0AdministracionProvider(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<Auth0AdministracionProvider> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<string> CrearUsuarioAsync(
            string correo,
            string contrasena,
            string nombre,
            string apellido,
            CancellationToken cancellationToken = default)
        {
            var cuerpo = new
            {
                email = correo,
                password = contrasena,
                email_verified = false,

                // Evita que Auth0 envie automaticamente el correo de verificacion.
                verify_email = false,
                name = $"{nombre} {apellido}".Trim(),
                given_name = nombre,
                family_name = apellido,
                connection = _configuration["Auth0:Connection"] ?? ConexionBaseDeDatos
            };

            using var respuesta = await EnviarAsync(HttpMethod.Post, RutaUsuarios, cuerpo, cancellationToken);

            if (respuesta.StatusCode == HttpStatusCode.Conflict)
            {
                throw new Auth0IntegracionException($"Ya existe un usuario en Auth0 con el correo '{correo}'.");
            }

            if (!respuesta.IsSuccessStatusCode)
            {
                throw new Auth0IntegracionException($"Auth0 no pudo crear el usuario: {await LeerDetalleAsync(respuesta, cancellationToken)}");
            }

            var contenido = await respuesta.Content.ReadAsStringAsync(cancellationToken);

            using var documento = JsonDocument.Parse(contenido);

            if (!documento.RootElement.TryGetProperty("user_id", out var userId) ||
                string.IsNullOrWhiteSpace(userId.GetString()))
            {
                throw new Auth0IntegracionException("Auth0 no devolvio el identificador del usuario creado.");
            }

            var identificador = userId.GetString();

            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation(
                    "Usuario {Correo} creado en Auth0 con el identificador {UserId}",
                    correo,
                    identificador);
            }

            return identificador!;
        }

        public async Task ActualizarUsuarioAsync(
            string auth0UserId,
            string correo,
            string nombre,
            string apellido,
            CancellationToken cancellationToken = default)
        {
            ValidarIdentificador(auth0UserId);

            var cuerpo = new
            {
                email = correo,
                name = $"{nombre} {apellido}".Trim(),
                given_name = nombre,
                family_name = apellido
            };

            using var respuesta = await EnviarAsync(
                HttpMethod.Patch,
                $"{RutaUsuarios}/{Uri.EscapeDataString(auth0UserId)}",
                cuerpo,
                cancellationToken);

            if (respuesta.StatusCode == HttpStatusCode.NotFound)
            {
                throw new Auth0IntegracionException($"El usuario '{auth0UserId}' no existe en Auth0.");
            }

            if (respuesta.StatusCode == HttpStatusCode.Conflict)
            {
                throw new Auth0IntegracionException($"Ya existe un usuario en Auth0 con el correo '{correo}'.");
            }

            if (!respuesta.IsSuccessStatusCode)
            {
                throw new Auth0IntegracionException($"Auth0 no pudo actualizar el usuario: {await LeerDetalleAsync(respuesta, cancellationToken)}");
            }

            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Usuario {UserId} actualizado en Auth0", auth0UserId);
            }
        }

        public async Task EliminarUsuarioAsync(string auth0UserId, CancellationToken cancellationToken = default)
        {
            ValidarIdentificador(auth0UserId);

            using var respuesta = await EnviarAsync(
                HttpMethod.Delete,
                $"{RutaUsuarios}/{Uri.EscapeDataString(auth0UserId)}",
                cuerpo: null,
                cancellationToken);

            // Si el usuario ya no existe, la operacion es idempotente.
            if (respuesta.StatusCode == HttpStatusCode.NotFound)
            {
                return;
            }

            if (!respuesta.IsSuccessStatusCode)
            {
                throw new Auth0IntegracionException($"Auth0 no pudo eliminar el usuario: {await LeerDetalleAsync(respuesta, cancellationToken)}");
            }

            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Usuario {UserId} eliminado de Auth0", auth0UserId);
            }
        }

        public async Task<List<RolDto>> ObtenerRolesAsync(CancellationToken cancellationToken = default)
        {
            using var respuesta = await EnviarAsync(
                HttpMethod.Get,
                $"{RutaRoles}?per_page={CantidadMaximaPorPagina}",
                cuerpo: null,
                cancellationToken);

            await AsegurarRespuestaExitosaAsync(respuesta, "obtener los roles", cancellationToken);

            return await LeerRolesAsync(respuesta, cancellationToken);
        }

        public async Task<List<RolDto>> ObtenerRolesDeUsuarioAsync(
            string auth0UserId,
            CancellationToken cancellationToken = default)
        {
            ValidarIdentificador(auth0UserId);

            using var respuesta = await EnviarAsync(
                HttpMethod.Get,
                $"{RutaUsuarios}/{Uri.EscapeDataString(auth0UserId)}/roles?per_page={CantidadMaximaPorPagina}",
                cuerpo: null,
                cancellationToken);

            if (respuesta.StatusCode == HttpStatusCode.NotFound)
            {
                throw new Auth0IntegracionException($"El usuario '{auth0UserId}' no existe en Auth0.");
            }

            await AsegurarRespuestaExitosaAsync(respuesta, "obtener los roles del usuario", cancellationToken);

            return await LeerRolesAsync(respuesta, cancellationToken);
        }

        public async Task ReemplazarRolesAsync(
            string auth0UserId,
            IReadOnlyCollection<string> rolesIds,
            CancellationToken cancellationToken = default)
        {
            ValidarIdentificador(auth0UserId);

            var rolesActuales = await ObtenerRolesDeUsuarioAsync(auth0UserId, cancellationToken);
            var idsActuales = rolesActuales.Select(rol => rol.Id).Distinct(StringComparer.Ordinal).ToList();
            var idsNuevos = rolesIds
                .Where(rolId => !string.IsNullOrWhiteSpace(rolId))
                .Distinct(StringComparer.Ordinal)
                .ToList();

            if (idsActuales.Count > 0)
            {
                using var respuestaEliminar = await EnviarAsync(
                    HttpMethod.Delete,
                    $"{RutaUsuarios}/{Uri.EscapeDataString(auth0UserId)}/roles",
                    new { roles = idsActuales },
                    cancellationToken);

                await AsegurarRespuestaExitosaAsync(respuestaEliminar, "quitar los roles del usuario", cancellationToken);
            }

            if (idsNuevos.Count == 0)
            {
                return;
            }

            using var respuestaAsignar = await EnviarAsync(
                HttpMethod.Post,
                $"{RutaUsuarios}/{Uri.EscapeDataString(auth0UserId)}/roles",
                new { roles = idsNuevos },
                cancellationToken);

            await AsegurarRespuestaExitosaAsync(respuestaAsignar, "asignar los roles al usuario", cancellationToken);

            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation(
                    "Roles del usuario {UserId} reemplazados en Auth0 por {Roles}",
                    auth0UserId,
                    string.Join(", ", idsNuevos));
            }
        }

        public async Task<int> ObtenerCantidadUsuariosConRolAsync(
            string nombreRol,
            CancellationToken cancellationToken = default)
        {
            var roles = await ObtenerRolesAsync(cancellationToken);

            var rol = roles.FirstOrDefault(rol =>
                string.Equals(rol.Nombre, nombreRol, StringComparison.OrdinalIgnoreCase));

            if (rol is null)
            {
                throw new Auth0IntegracionException($"El rol '{nombreRol}' no existe en Auth0.");
            }

            using var respuesta = await EnviarAsync(
                HttpMethod.Get,
                $"{RutaRoles}/{Uri.EscapeDataString(rol.Id)}/users?per_page=1&include_totals=true",
                cuerpo: null,
                cancellationToken);

            await AsegurarRespuestaExitosaAsync(respuesta, "contar los usuarios del rol", cancellationToken);

            var contenido = await respuesta.Content.ReadAsStringAsync(cancellationToken);

            using var documento = JsonDocument.Parse(contenido);

            if (!documento.RootElement.TryGetProperty("total", out var total) ||
                !total.TryGetInt32(out var cantidad))
            {
                throw new Auth0IntegracionException("Auth0 no devolvio la cantidad de usuarios del rol.");
            }

            return cantidad;
        }

        private static async Task<List<RolDto>> LeerRolesAsync(
            HttpResponseMessage respuesta,
            CancellationToken cancellationToken)
        {
            var contenido = await respuesta.Content.ReadAsStringAsync(cancellationToken);

            using var documento = JsonDocument.Parse(contenido);

            if (documento.RootElement.ValueKind != JsonValueKind.Array)
            {
                return new List<RolDto>();
            }

            var roles = new List<RolDto>();

            foreach (var elemento in documento.RootElement.EnumerateArray())
            {
                var id = elemento.TryGetProperty("id", out var idElemento) ? idElemento.GetString() : null;
                var nombre = elemento.TryGetProperty("name", out var nombreElemento) ? nombreElemento.GetString() : null;

                if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(nombre))
                {
                    continue;
                }

                roles.Add(new RolDto { Id = id, Nombre = nombre });
            }

            return roles;
        }

        private static async Task AsegurarRespuestaExitosaAsync(
            HttpResponseMessage respuesta,
            string operacion,
            CancellationToken cancellationToken)
        {
            if (respuesta.IsSuccessStatusCode)
            {
                return;
            }

            throw new Auth0IntegracionException(
                $"Auth0 no pudo {operacion}: {await LeerDetalleAsync(respuesta, cancellationToken)}");
        }

        private async Task<HttpResponseMessage> EnviarAsync(
            HttpMethod metodo,
            string ruta,
            object? cuerpo,
            CancellationToken cancellationToken)
        {
            var token = await ObtenerTokenAsync(cancellationToken);
            var authority = ObtenerAuthority();

            using var peticion = new HttpRequestMessage(metodo, $"{authority.TrimEnd('/')}/{ruta}");
            peticion.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            if (cuerpo is not null)
            {
                peticion.Content = JsonContent.Create(cuerpo);
            }

            return await _httpClient.SendAsync(peticion, cancellationToken);
        }

        private string ObtenerAuthority()
        {
            var authority = _configuration["Auth0:Authority"];

            if (string.IsNullOrWhiteSpace(authority))
            {
                throw new Auth0IntegracionException("Falta la configuracion Auth0:Authority para invocar la Management API.");
            }

            return authority;
        }

        private async Task<string> ObtenerTokenAsync(CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(_tokenDeAcceso) && _expiracionDelToken > DateTimeOffset.UtcNow.AddMinutes(1))
            {
                return _tokenDeAcceso;
            }

            var authority = ObtenerAuthority();
            var clientId = _configuration["Auth0:ManagementClientId"];
            var clientSecret = _configuration["Auth0:ManagementClientSecret"];
            var audience = _configuration["Auth0:ManagementAudience"] ?? $"{authority.TrimEnd('/')}/api/v2/";

            if (string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(clientSecret))
            {
                throw new Auth0IntegracionException(
                    "Falta la configuracion de la Management API de Auth0 (Auth0:ManagementClientId y Auth0:ManagementClientSecret).");
            }

            using var peticion = new HttpRequestMessage(HttpMethod.Post, $"{authority.TrimEnd('/')}/oauth/token");
            peticion.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["grant_type"] = "client_credentials",
                ["client_id"] = clientId,
                ["client_secret"] = clientSecret,
                ["audience"] = audience
            });

            using var respuesta = await _httpClient.SendAsync(peticion, cancellationToken);

            if (!respuesta.IsSuccessStatusCode)
            {
                throw new Auth0IntegracionException(
                    $"No se pudo obtener el token de la Management API de Auth0: {await LeerDetalleAsync(respuesta, cancellationToken)}");
            }

            var contenido = await respuesta.Content.ReadAsStringAsync(cancellationToken);

            using var documento = JsonDocument.Parse(contenido);

            var token = documento.RootElement.GetProperty("access_token").GetString();

            if (string.IsNullOrWhiteSpace(token))
            {
                throw new Auth0IntegracionException("Auth0 devolvio un token de acceso vacio para la Management API.");
            }

            _tokenDeAcceso = token;
            var segundos = documento.RootElement.TryGetProperty("expires_in", out var expira) ? expira.GetInt32() : 3600;
            _expiracionDelToken = DateTimeOffset.UtcNow.AddSeconds(segundos);

            return token;
        }

        private static void ValidarIdentificador(string auth0UserId)
        {
            if (string.IsNullOrWhiteSpace(auth0UserId))
            {
                throw new Auth0IntegracionException("El identificador del usuario en Auth0 es obligatorio.");
            }
        }

        private static async Task<string> LeerDetalleAsync(HttpResponseMessage respuesta, CancellationToken cancellationToken)
        {
            var contenido = await respuesta.Content.ReadAsStringAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(contenido))
            {
                return $"HTTP {(int)respuesta.StatusCode} {respuesta.ReasonPhrase}";
            }

            try
            {
                using var documento = JsonDocument.Parse(contenido);

                if (documento.RootElement.TryGetProperty("error_description", out var descripcion))
                {
                    return descripcion.GetString() ?? contenido;
                }

                if (documento.RootElement.TryGetProperty("message", out var mensaje))
                {
                    return mensaje.GetString() ?? contenido;
                }
            }
            catch (JsonException)
            {
                return contenido;
            }

            return contenido;
        }
    }
}
