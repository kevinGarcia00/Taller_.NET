using System;
using Modules.Sensor.Domain.Excepciones;

namespace Modules.Sensor.Domain
{
    public class Panel
    {
        public const int LongitudMaximaNombre = 100;
        public const int LongitudMaximaVidaUtil = 50;

        public int Id { get; }
        public string Nombre { get; }
        public DateTime FechaCreacion { get; }
        public EstadoPanel Estado { get; }
        public bool Encendido { get; }
        public bool VolcarEnergia { get; }
        public decimal Potencia { get; }
        public string VidaUtil { get; }
        public int IdNodo { get; }

        public Panel(
            int id,
            string nombre,
            DateTime fechaCreacion,
            EstadoPanel estado,
            bool encendido,
            decimal potencia,
            string vidaUtil,
            int idNodo,
            bool volcarEnergia = false)
        {
            if (id <= 0)
            {
                throw new PanelInvalidoException("El identificador del panel debe ser mayor a cero.");
            }

            ValidarNombre(nombre);
            ValidarFechaCreacion(fechaCreacion);
            ValidarEstado(estado);
            ValidarPotencia(potencia);
            ValidarVidaUtil(vidaUtil);

            if (idNodo <= 0)
            {
                throw new PanelInvalidoException("El panel debe pertenecer a un nodo valido.");
            }

            Id = id;
            Nombre = nombre.Trim();
            FechaCreacion = fechaCreacion;
            Estado = estado;
            Encendido = encendido;
            VolcarEnergia = volcarEnergia;
            Potencia = potencia;
            VidaUtil = vidaUtil.Trim();
            IdNodo = idNodo;
        }

        public bool PuedeCambiarA(AccionEstadoPanel accion)
        {
            if (!Enum.IsDefined(accion))
            {
                throw new PanelInvalidoException("La accion sobre el panel no es valida.");
            }

            return accion == AccionEstadoPanel.Encender
                ? Estado != EstadoPanel.Encendido
                : Estado != EstadoPanel.Apagado;
        }

        public bool PuedeCambiarVolcadoEnergia(bool volcarEnergia)
        {
            return VolcarEnergia != volcarEnergia;
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

        private static void ValidarFechaCreacion(DateTime fechaCreacion)
        {
            if (fechaCreacion == default)
            {
                throw new PanelInvalidoException("La fecha de creacion del panel es obligatoria.");
            }

            if (fechaCreacion > DateTime.UtcNow)
            {
                throw new PanelInvalidoException("La fecha de creacion del panel no puede ser futura.");
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
