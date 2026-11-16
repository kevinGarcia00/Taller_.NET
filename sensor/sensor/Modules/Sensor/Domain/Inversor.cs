using System;
using Modules.Sensor.Domain.Excepciones;

namespace Modules.Sensor.Domain
{
    public class Inversor
    {
        public const int LongitudMaximaNombre = 100;

        public int Id { get; }
        public string Nombre { get; }
        public DateTime FechaCreacion { get; }

        public Inversor(int id, string nombre, DateTime fechaCreacion)
        {
            if (id <= 0)
            {
                throw new InversorInvalidoException("El identificador del inversor debe ser mayor a cero.");
            }

            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new InversorInvalidoException("El nombre del inversor es obligatorio.");
            }

            if (nombre.Length > LongitudMaximaNombre)
            {
                throw new InversorInvalidoException($"El nombre del inversor no puede superar los {LongitudMaximaNombre} caracteres.");
            }

            if (fechaCreacion == default)
            {
                throw new InversorInvalidoException("La fecha de creacion del inversor es obligatoria.");
            }

            if (fechaCreacion > DateTime.UtcNow)
            {
                throw new InversorInvalidoException("La fecha de creacion del inversor no puede ser futura.");
            }

            Id = id;
            Nombre = nombre.Trim();
            FechaCreacion = fechaCreacion;
        }
    }
}
