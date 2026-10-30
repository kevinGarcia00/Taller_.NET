using System;
using Taller_.NET.Modulos.ParqueSolar.Domain.Excepciones;

namespace Taller_.NET.Modulos.ParqueSolar.Domain
{
    public class Nodo
    {
        public const int LongitudMaximaNombre = 100;
        public const int LongitudMaximaUbicacionParque = 200;

        private ContextoParque? _contextoParque;

        public int Id { get; private set; }
        public string Nombre { get; private set; } = string.Empty;
        public string UbicacionParque { get; private set; } = string.Empty;
        public DateTime FechaCreacion { get; private set; }
        public decimal EnergiaGenerada { get; private set; }
        public int? InversorId { get; private set; }
        public Inversor? Inversor { get; private set; }
        public int ParqueSolarId { get; private set; }

        public ContextoParque? ContextoParque =>
            _contextoParque ??= ParqueSolarId > 0 ? new ContextoParque(ParqueSolarId) : null;

        private Nodo()
        {
        }

        public Nodo(string nombre, string ubicacionParque, int? inversorId = null)
        {
            ValidarNombre(nombre);
            ValidarUbicacionParque(ubicacionParque);
            ValidarInversorId(inversorId);

            Nombre = nombre.Trim();
            UbicacionParque = ubicacionParque.Trim();
            InversorId = inversorId;
            FechaCreacion = DateTime.UtcNow;
            EnergiaGenerada = 0;
        }

        public void AsignarContexto(ContextoParque contexto)
        {
            if (contexto is null)
            {
                throw new NodoInvalidoException("El contexto del parque solar es obligatorio.");
            }

            _contextoParque = contexto;
            ParqueSolarId = contexto.ParqueSolarId;
        }

        public void Actualizar(string nombre, string ubicacionParque, int? inversorId)
        {
            ValidarNombre(nombre);
            ValidarUbicacionParque(ubicacionParque);
            ValidarInversorId(inversorId);

            Nombre = nombre.Trim();
            UbicacionParque = ubicacionParque.Trim();
            InversorId = inversorId;
        }

        private static void ValidarNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new NodoInvalidoException("El nombre del nodo es obligatorio.");
            }

            if (nombre.Length > LongitudMaximaNombre)
            {
                throw new NodoInvalidoException($"El nombre del nodo no puede superar los {LongitudMaximaNombre} caracteres.");
            }
        }

        private static void ValidarUbicacionParque(string ubicacionParque)
        {
            if (string.IsNullOrWhiteSpace(ubicacionParque))
            {
                throw new NodoInvalidoException("La ubicacion del nodo en el parque es obligatoria.");
            }

            if (ubicacionParque.Length > LongitudMaximaUbicacionParque)
            {
                throw new NodoInvalidoException($"La ubicacion del nodo no puede superar los {LongitudMaximaUbicacionParque} caracteres.");
            }
        }

        private static void ValidarInversorId(int? inversorId)
        {
            if (inversorId.HasValue && inversorId.Value <= 0)
            {
                throw new NodoInvalidoException("El inversor asociado al nodo no es valido.");
            }
        }
    }
}
