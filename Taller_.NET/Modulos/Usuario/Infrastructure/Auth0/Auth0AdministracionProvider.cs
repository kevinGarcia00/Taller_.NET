using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Taller_.NET.Modulos.Usuario.Application.Excepciones;
using Taller_.NET.Modulos.Usuario.Application.Puertos;

namespace Taller_.NET.Modulos.Usuario.Infrastructure.Auth0
{
    /// <summary>
    /// Cliente de la Management API de Auth0. Crea los usuarios en el proveedor de
    /// identidad y les asigna los roles del sistema.
    /// </summary>
    public class Auth0AdministracionProvider : IAuth0AdministracionProvider
    {
        private const string ConexionBaseDeDatos = "Username-Password-Authentication";

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
            IEnumerable<string> roles,
            CancellationToken cancellationToken = default)
        {
            var token = await ObtenerTokenAsync(cancellationToken);

            var cuerpo = new
            {
                email = correo,
                password = contrasena,
                email_verified = true,
                name = $"{nombre} {apellido}".Trim(),
                given_name = nombre,
                family_name = apellido,
                connection = _configuration["Auth0:Connection"] ?? ConexionBaseDeDatos,

                // Los roles viajan en app_metadata: es donde Auth0 guarda los roles del
                // usuario y evita depender del endpoint adicional de asignacion de roles.
                app_metadata = new
                {
                    roles = roles.Where(rol => !string.IsNullOrWhiteSpace(rol)).Distinct().ToList()
                }
            };

            using var respuesta = await EnviarAsync(HttpMethod.Post, "api/v2/users", token, cuerpo, cancellationToken);

            if (respuesta.StatusCode == HttpStatusCode.Conflict)
            {
                throw new Auth0IntegracionException($"Ya existe un usuario en Auth0 con el correo '{correo}'.");
            }

            if (!respuesta.IsSuccessStatusCode)
            {
                throw new Auth0IntegracionException($"Auth0 no pudo crear el usuario: {await LeerDetalleAsync(respuesta)}");
            }

            var contenido = await respuesta.Content.ReadAsStringAsync(cancellationToken);

            using var documento = JsonDocument.Parse(contenido);

            if (!documento.RootElement.TryGetProperty("user_id", out var userId) || string.IsNullOrWhiteSpace(userId.GetString()))
            {
                throw new Auth0IntegracionException("Auth0 no devolvio el identificador del usuario creado.");
            }

            _logger.LogInformation("Usuario {Correo} creado en Auth0 con el identificador {UserId}", correo, userId.GetString());

            return userId.GetString()!;
        }

        private async Task<HttpResponseMessage> EnviarAsync(
            HttpMethod metodo,
            string ruta,
            string token,
            object cuerpo,
            CancellationToken cancellationToken)
        {
            var authority = ObtenerAuthority();

            using var peticion = new HttpRequestMessage(metodo, $"{authority.TrimEnd('/')}/{ruta}");
            peticion.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            peticion.Content = JsonContent.Create(cuerpo);

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
                    $"No se pudo obtener el token de la Management API de Auth0: {await LeerDetalleAsync(respuesta)}");
            }

            var contenido = await respuesta.Content.ReadAsStringAsync(cancellationToken);

            using var documento = JsonDocument.Parse(contenido);

            _tokenDeAcceso = documento.RootElement.GetProperty("access_token").GetString();
            var segundos = documento.RootElement.TryGetProperty("expires_in", out var expira) ? expira.GetInt32() : 3600;
            _expiracionDelToken = DateTimeOffset.UtcNow.AddSeconds(segundos);

            return _tokenDeAcceso!;
        }

        private static async Task<string> LeerDetalleAsync(HttpResponseMessage respuesta)
        {
            var contenido = await respuesta.Content.ReadAsStringAsync();

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
