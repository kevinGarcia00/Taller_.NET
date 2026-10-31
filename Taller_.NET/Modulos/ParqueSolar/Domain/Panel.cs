using System;
using Taller_.NET.Modulos.ParqueSolar.Domain.Excepciones;

namespace Taller_.NET.Modulos.ParqueSolar.Domain
{
    public class Panel
    {
        public const int LongitudMaximaNombre = 100;
        public const int LongitudMaximaVidaUtil = 50;

        public int Id { get; private set; }
        public string Nombre { get; private set; } = string.Empty;
        public DateTime FechaCreacion { get; private set; }
        public EstadoPanel Estado { get; private set; }
        public decimal Potencia { get; private set; }
        public string VidaUtil { get; private set; } = string.Empty;
        public int NodoId { get; private set; }
        public Nodo? Nodo { get; private set; }

        private Panel()
        {
        }

        public Panel(string nombre, EstadoPanel estado, decimal potencia, string vidaUtil)
        {
            ValidarNombre(nombre);
            ValidarEstado(estado);
            ValidarPotencia(potencia);
            ValidarVidaUtil(vidaUtil);

            Nombre = nombre.Trim();
            Estado = estado;
            Potencia = potencia;
            VidaUtil = vidaUtil.Trim();
            FechaCreacion = DateTime.UtcNow;
        }

        public void AsignarNodo(Nodo nodo)
        {
            if (nodo is null)
            {
                throw new PanelInvalidoException("El nodo del panel es obligatorio.");
            }

            if (nodo.Id <= 0)
            {
                throw new PanelInvalidoException("El nodo del panel debe estar registrado.");
            }

            Nodo = nodo;
            NodoId = nodo.Id;
        }

        public void ActualizarDatos(string nombre, EstadoPanel estado, decimal potencia, string vidaUtil)
        {
            ValidarNombre(nombre);
            ValidarEstado(estado);
            ValidarPotencia(potencia);
            ValidarVidaUtil(vidaUtil);

            Nombre = nombre.Trim();
            Estado = estado;
            Potencia = potencia;
            VidaUtil = vidaUtil.Trim();
        }

        private static void ValidarNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new PanelInvalidoException("El nombre del panel es obligatorio.");
            }

            if (nombre.Length > LongitudMaximaNombre)
            {
                throw new PanelInvalidoException($"El nombre del panel no puede superar los {LongitudMaximaNombre} caracteres.");
            }
        }

        private static void ValidarEstado(EstadoPanel estado)
        {
            if (!Enum.IsDefined(estado))
            {
                throw new PanelInvalidoException("El estado del panel no es valido.");
            }
        }

        private static void ValidarPotencia(decimal potencia)
        {
            if (potencia <= 0)
            {
                throw new PanelInvalidoException("La potencia del panel debe ser mayor a cero.");
            }
        }

        private static void ValidarVidaUtil(string vidaUtil)
        {
            if (string.IsNullOrWhiteSpace(vidaUtil))
            {
                throw new PanelInvalidoException("La vida util del panel es obligatoria.");
            }

            if (vidaUtil.Length > LongitudMaximaVidaUtil)
            {
                throw new PanelInvalidoException($"La vida util del panel no puede superar los {LongitudMaximaVidaUtil} caracteres.");
            }
        }
    }
}
