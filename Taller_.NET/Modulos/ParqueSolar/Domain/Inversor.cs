using System;
using Taller_.NET.Modulos.ParqueSolar.Domain.Excepciones;

namespace Taller_.NET.Modulos.ParqueSolar.Domain
{
    public class Inversor
    {
        public const int LongitudMaximaNombre = 100;

        public int Id { get; private set; }
        public string Nombre { get; private set; } = string.Empty;
        public DateTime FechaCreacion { get; private set; }

        private Inversor()
        {
        }

        public Inversor(string nombre)
        {
            ValidarNombre(nombre);

            Nombre = nombre.Trim();
            FechaCreacion = DateTime.UtcNow;
        }

        private static void ValidarNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new InversorInvalidoException("El nombre del inversor es obligatorio.");
            }

            if (nombre.Length > LongitudMaximaNombre)
            {
                throw new InversorInvalidoException($"El nombre del inversor no puede superar los {LongitudMaximaNombre} caracteres.");
            }
        }
    }
}
