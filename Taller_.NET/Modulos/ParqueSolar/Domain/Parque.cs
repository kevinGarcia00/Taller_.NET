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
            ValidarNombre(nombre);
            ValidarUbicacion(ubicacion);
            ValidarCantMaxNodos(cantMaxNodos);

            Nombre = nombre.Trim();
            Ubicacion = ubicacion.Trim();
            CantMaxNodos = cantMaxNodos;
            FechaCreacion = DateTime.UtcNow;
        }

        public void Modificar(string nombre, string ubicacion, int cantMaxNodos)
        {
            ValidarNombre(nombre);
            ValidarUbicacion(ubicacion);
            ValidarCantMaxNodos(cantMaxNodos);

            Nombre = nombre.Trim();
            Ubicacion = ubicacion.Trim();
            CantMaxNodos = cantMaxNodos;
        }

        public void ValidarCapacidadNodos(int nodosRegistrados)
        {
            ValidarCapacidadNodos(nodosRegistrados, CantMaxNodos);
        }

        public void ValidarCapacidadNodos(int nodosRegistrados, int cantMaxNodos)
        {
            ValidarNodosRegistrados(nodosRegistrados);
            ValidarCantMaxNodos(cantMaxNodos);

            if (nodosRegistrados > cantMaxNodos)
            {
                throw new ParqueInvalidoException(
                    $"El parque tiene {nodosRegistrados} nodos registrados y no puede admitir una capacidad maxima de {cantMaxNodos} nodos.");
            }
        }

        public int ObtenerCantidadNodosActuales(int nodosRegistrados)
        {
            ValidarNodosRegistrados(nodosRegistrados);

            return nodosRegistrados;
        }

        private static void ValidarNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ParqueInvalidoException("El nombre del parque es obligatorio.");
            }

            if (nombre.Length > LongitudMaximaNombre)
            {
                throw new ParqueInvalidoException($"El nombre del parque no puede superar los {LongitudMaximaNombre} caracteres.");
            }
        }

        private static void ValidarUbicacion(string ubicacion)
        {
            if (string.IsNullOrWhiteSpace(ubicacion))
            {
                throw new ParqueInvalidoException("La ubicacion del parque es obligatoria.");
            }

            if (ubicacion.Length > LongitudMaximaUbicacion)
            {
                throw new ParqueInvalidoException($"La ubicacion del parque no puede superar los {LongitudMaximaUbicacion} caracteres.");
            }
        }

        private static void ValidarCantMaxNodos(int cantMaxNodos)
        {
            if (cantMaxNodos <= 0)
            {
                throw new ParqueInvalidoException("La cantidad maxima de nodos debe ser mayor a cero.");
            }
        }

        private static void ValidarNodosRegistrados(int nodosRegistrados)
        {
            if (nodosRegistrados < 0)
            {
                throw new ParqueInvalidoException("La cantidad de nodos registrados no puede ser negativa.");
            }
        }
    }
}
