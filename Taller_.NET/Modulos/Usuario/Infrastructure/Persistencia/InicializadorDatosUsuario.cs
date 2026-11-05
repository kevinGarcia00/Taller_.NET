using Taller_.NET.Modulos.Usuario.Application.Puertos;

namespace Taller_.NET.Modulos.Usuario.Infrastructure.Persistencia
{
    /// <summary>
    /// Garantiza que la base local tenga el usuario administrador inicial vinculado
    /// a Auth0. Se ejecuta una sola vez al iniciar la aplicacion. El rol del usuario
    /// se administra exclusivamente en Auth0.
    /// </summary>
    public class InicializadorDatosUsuario
    {
        public const string CorreoAdministradorInicial = "admin@admin.com";
        public const string Auth0UserIdAdministradorInicial = "auth0|6ae636752d220ecd31c20d5d";

        private const string NombreAdministradorInicial = "Administrador";
        private const string ApellidoAdministradorInicial = "Sistema";

        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ILogger<InicializadorDatosUsuario> _logger;

        public InicializadorDatosUsuario(
            IUsuarioRepository usuarioRepository,
            ILogger<InicializadorDatosUsuario> logger)
        {
            _usuarioRepository = usuarioRepository;
            _logger = logger;
        }

        public async Task AsegurarAdministradorInicialAsync(CancellationToken cancellationToken = default)
        {
            var administradorExistente =
                await _usuarioRepository.ObtenerPorAuth0UserIdAsync(Auth0UserIdAdministradorInicial, cancellationToken)
                ?? await _usuarioRepository.ObtenerPorCorreoAsync(CorreoAdministradorInicial, cancellationToken);

            if (administradorExistente is not null)
            {
                await CompletarAdministradorExistenteAsync(administradorExistente, cancellationToken);

                return;
            }

            var administrador = new Usuario(
                NombreAdministradorInicial,
                ApellidoAdministradorInicial,
                CorreoAdministradorInicial,
                string.Empty);

            administrador.VincularConAuth0(Auth0UserIdAdministradorInicial);

            await _usuarioRepository.AgregarAsync(administrador, cancellationToken);
            await _usuarioRepository.GuardarCambiosAsync(cancellationToken);

            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation(
                    "Se creo el usuario administrador inicial {Correo} vinculado a Auth0 con el identificador {Auth0UserId}.",
                    CorreoAdministradorInicial,
                    Auth0UserIdAdministradorInicial);
            }
        }

        private async Task CompletarAdministradorExistenteAsync(
            Usuario administrador,
            CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(administrador.Auth0UserId))
            {
                if (!string.Equals(
                        administrador.Auth0UserId,
                        Auth0UserIdAdministradorInicial,
                        StringComparison.Ordinal))
                {
                    if (_logger.IsEnabled(LogLevel.Warning))
                    {
                        _logger.LogWarning(
                            "El usuario {Correo} ya existe pero esta vinculado a otro identificador de Auth0 ({Auth0UserIdActual}).",
                            CorreoAdministradorInicial,
                            administrador.Auth0UserId);
                    }
                }

                return;
            }

            administrador.VincularConAuth0(Auth0UserIdAdministradorInicial);

            await _usuarioRepository.ActualizarAsync(administrador, cancellationToken);
            await _usuarioRepository.GuardarCambiosAsync(cancellationToken);

            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation(
                    "Se completo el vinculo con Auth0 del usuario administrador inicial {Correo}.",
                    CorreoAdministradorInicial);
            }
        }
    }
}
