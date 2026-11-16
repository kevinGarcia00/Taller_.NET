using Modules.Sensor.Domain.Excepciones;

namespace Modules.Sensor.Domain
{
    public class Nodo
    {
        public const int LongitudMaximaNombre = 100;

        public int Id { get; }
        public string Nombre { get; }
        public int IdInversor { get; }

        public Nodo(int id, string nombre, int idInversor)
        {
            if (id <= 0)
            {
                throw new NodoInvalidoException("El identificador del nodo debe ser mayor a cero.");
            }

            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new NodoInvalidoException("El nombre del nodo es obligatorio.");
            }

            if (nombre.Length > LongitudMaximaNombre)
            {
                throw new NodoInvalidoException($"El nombre del nodo no puede superar los {LongitudMaximaNombre} caracteres.");
            }

            if (idInversor <= 0)
            {
                throw new NodoInvalidoException("El nodo debe pertenecer a un inversor valido.");
            }

            Id = id;
            Nombre = nombre.Trim();
            IdInversor = idInversor;
        }
    }
}
