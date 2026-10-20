using System;
using Taller_.NET.Modulos.ParqueSolar.Domain.Excepciones;

namespace Taller_.NET.Modulos.ParqueSolar.Domain
{
    public class Parque
    {
        public const int LongitudMaximaNombre = 100;
        public const int LongitudMaximaUbicacion = 100;

        public int Id { get; private set; }
        public string Nombre { get; private set; }
        public string Ubicacion { get; private set; }
        public DateTime FechaCreacion { get; private set; }
        public int CantMaxNodos { get; private set; }

        public Parque(string nombre, string ubicacion, int cantMaxNodos)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ParqueInvalidoException("El nombre del parque es obligatorio.");
            }

            if (nombre.Length > LongitudMaximaNombre)
            {
                throw new ParqueInvalidoException($"El nombre del parque no puede superar los {LongitudMaximaNombre} caracteres.");
            }

            if (string.IsNullOrWhiteSpace(ubicacion))
            {
                throw new ParqueInvalidoException("La ubicacion del parque es obligatoria.");
            }

            if (ubicacion.Length > LongitudMaximaUbicacion)
            {
                throw new ParqueInvalidoException($"La ubicacion del parque no puede superar los {LongitudMaximaUbicacion} caracteres.");
            }

            if (cantMaxNodos <= 0)
            {
                throw new ParqueInvalidoException("La cantidad maxima de nodos debe ser mayor a cero.");
            }

            Nombre = nombre.Trim();
            Ubicacion = ubicacion.Trim();
            CantMaxNodos = cantMaxNodos;
            FechaCreacion = DateTime.UtcNow;
        }
    }
}
