using Taller_.NET.Modulos.Usuario.Excepciones;

namespace Taller_.NET.Modulos.Usuario
{
    public class Rol
    {
        public const int LongitudMaximaNombre = 25;
        public const string NombreAdministrador = "Administrador";
        public const string NombreMantenimiento = "Mantenimiento";

        public long Id { get; private set; }
        public string Nombre { get; private set; }

        public Rol(long id, string nombre)
        {
            Id = id;
            Nombre = ValidarNombre(nombre);
        }

        private static string ValidarNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new RolInvalidoException("El nombre del rol es obligatorio.");
            }

            if (nombre.Length > LongitudMaximaNombre)
            {
                throw new RolInvalidoException($"El nombre del rol no puede superar los {LongitudMaximaNombre} caracteres.");
            }

            return nombre.Trim();
        }
    }
}
