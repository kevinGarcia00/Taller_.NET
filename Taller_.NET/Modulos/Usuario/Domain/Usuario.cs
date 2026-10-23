using Taller_.NET.Modulos.Usuario.Excepciones;

// La entidad vive en el namespace raiz del modulo porque un tipo llamado "Usuario"
// no puede resolverse desde los sub-namespaces del modulo: el namespace homonimo
// "Taller_.NET.Modulos.Usuario" tiene prioridad sobre el tipo importado.
namespace Taller_.NET.Modulos.Usuario
{
    public class Usuario
    {
        public const int LongitudMaximaNombre = 100;
        public const int LongitudMaximaApellido = 100;
        public const int LongitudMaximaCorreo = 100;
        public const int LongitudMaximaTelefono = 25;
        public const int LongitudMaximaAuth0UserId = 100;

        public Guid Id { get; private set; }
        public string Nombre { get; private set; }
        public string Apellido { get; private set; }
        public string Correo { get; private set; }
        public string Telefono { get; private set; }

        /// <summary>
        /// Identificador del usuario en Auth0 (user_id). Es null en los usuarios
        /// dados de alta antes de la integracion con el proveedor de identidad.
        /// </summary>
        public string? Auth0UserId { get; private set; }

        private readonly List<Rol> _roles = new();
        public IReadOnlyCollection<Rol> Roles => _roles.AsReadOnly();

        public Usuario(string nombre, string apellido, string correo, string telefono)
        {
            Id = Guid.NewGuid();

            Nombre = ValidarNombre(nombre);
            Apellido = ValidarApellido(apellido);
            Correo = ValidarCorreo(correo);
            Telefono = ValidarTelefono(telefono);
        }

        public void VincularConAuth0(string auth0UserId)
        {
            if (string.IsNullOrWhiteSpace(auth0UserId))
            {
                throw new UsuarioInvalidoException("El identificador del usuario en Auth0 es obligatorio.");
            }

            Auth0UserId = auth0UserId.Trim();
        }

        public void AsignarRol(Rol rol)
        {
            if (rol is null)
            {
                throw new UsuarioInvalidoException("El rol a asignar es obligatorio.");
            }

            if (_roles.Any(rolExistente => rolExistente.Id == rol.Id))
            {
                return;
            }

            _roles.Add(rol);
        }

        public void ActualizarDatosPersonales(string nombre, string apellido, string correo, string telefono)
        {
            // Se validan todos los datos antes de asignarlos para que un error no
            // deje al usuario modificado a medias.
            var nombreValidado = ValidarNombre(nombre);
            var apellidoValidado = ValidarApellido(apellido);
            var correoValidado = ValidarCorreo(correo);
            var telefonoValidado = ValidarTelefono(telefono);

            Nombre = nombreValidado;
            Apellido = apellidoValidado;
            Correo = correoValidado;
            Telefono = telefonoValidado;
        }

        public void ActualizarRoles(IEnumerable<Rol> nuevosRoles)
        {
            if (nuevosRoles is null)
            {
                throw new UsuarioInvalidoException("La coleccion de roles es obligatoria.");
            }

            var roles = nuevosRoles.Where(rol => rol is not null).DistinctBy(rol => rol.Id).ToList();

            if (roles.Count == 0)
            {
                throw new UsuarioInvalidoException("El usuario debe tener al menos un rol asignado.");
            }

            _roles.Clear();
            _roles.AddRange(roles);
        }

        public bool TieneRol(string nombreRol)
        {
            if (string.IsNullOrWhiteSpace(nombreRol))
            {
                return false;
            }

            return _roles.Any(rol => string.Equals(rol.Nombre, nombreRol.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        private static string ValidarNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new UsuarioInvalidoException("El nombre del usuario es obligatorio.");
            }

            if (nombre.Length > LongitudMaximaNombre)
            {
                throw new UsuarioInvalidoException($"El nombre del usuario no puede superar los {LongitudMaximaNombre} caracteres.");
            }

            return nombre.Trim();
        }

        private static string ValidarApellido(string apellido)
        {
            if (string.IsNullOrWhiteSpace(apellido))
            {
                throw new UsuarioInvalidoException("El apellido del usuario es obligatorio.");
            }

            if (apellido.Length > LongitudMaximaApellido)
            {
                throw new UsuarioInvalidoException($"El apellido del usuario no puede superar los {LongitudMaximaApellido} caracteres.");
            }

            return apellido.Trim();
        }

        private static string ValidarCorreo(string correo)
        {
            if (string.IsNullOrWhiteSpace(correo))
            {
                throw new UsuarioInvalidoException("El correo del usuario es obligatorio.");
            }

            if (correo.Length > LongitudMaximaCorreo)
            {
                throw new UsuarioInvalidoException($"El correo del usuario no puede superar los {LongitudMaximaCorreo} caracteres.");
            }

            var correoNormalizado = correo.Trim().ToLowerInvariant();

            if (!TieneFormatoCorreoValido(correoNormalizado))
            {
                throw new UsuarioInvalidoException("El correo del usuario debe cumplir con el formato usuario@dominio.com.");
            }

            return correoNormalizado;
        }

        private static bool TieneFormatoCorreoValido(string correo)
        {
            var separadores = correo.Split('@');

            if (separadores.Length != 2)
            {
                return false;
            }

            return separadores[0].Length > 0 && separadores[1].Length > 0 && !correo.Contains(' ');
        }

        private static string ValidarTelefono(string telefono)
        {
            if (string.IsNullOrWhiteSpace(telefono))
            {
                return string.Empty;
            }

            if (telefono.Length > LongitudMaximaTelefono)
            {
                throw new UsuarioInvalidoException($"El telefono del usuario no puede superar los {LongitudMaximaTelefono} caracteres.");
            }

            return telefono.Trim();
        }
    }
}
