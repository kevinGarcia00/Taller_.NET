using System;
using Taller_.NET.Modulos.Historico.Domain.Excepciones;

namespace Taller_.NET.Modulos.Historico.Domain
{
    public class HistoricoMantenibilidad
    {
        public const int LongitudMaximaObservaciones = 500;

        public int Id { get; private set; }
        public Mantenedor Mantenedor { get; private set; } = null!;
        public DateTime Fecha { get; private set; }
        public string Observaciones { get; private set; } = string.Empty;
        public EstadoPanelRegistrado EstadoInicial { get; private set; }
        public EstadoPanelRegistrado EstadoResultante { get; private set; }
        public ContextoPanel ContextoPanel { get; private set; } = null!;

        private HistoricoMantenibilidad()
        {
        }

        public HistoricoMantenibilidad(
            Mantenedor mantenedor,
            DateTime fecha,
            string observaciones,
            EstadoPanelRegistrado estadoInicial,
            EstadoPanelRegistrado estadoResultante,
            ContextoPanel contextoPanel)
        {
            if (mantenedor is null)
            {
                throw new HistoricoMantenibilidadInvalidoException("El mantenedor es obligatorio.");
            }

            if (contextoPanel is null)
            {
                throw new HistoricoMantenibilidadInvalidoException("El contexto del panel es obligatorio.");
            }

            ValidarObservaciones(observaciones);
            ValidarFecha(fecha);
            ValidarEstado(estadoInicial, "inicial");
            ValidarEstado(estadoResultante, "resultante");

            Mantenedor = mantenedor;
            Fecha = fecha.Date;
            Observaciones = observaciones.Trim();
            EstadoInicial = estadoInicial;
            EstadoResultante = estadoResultante;
            ContextoPanel = contextoPanel;
        }

        private static void ValidarObservaciones(string observaciones)
        {
            if (string.IsNullOrWhiteSpace(observaciones))
            {
                throw new HistoricoMantenibilidadInvalidoException("Las observaciones son obligatorias.");
            }

            if (observaciones.Trim().Length > LongitudMaximaObservaciones)
            {
                throw new HistoricoMantenibilidadInvalidoException(
                    $"Las observaciones no pueden superar los {LongitudMaximaObservaciones} caracteres.");
            }
        }

        private static void ValidarFecha(DateTime fecha)
        {
            if (fecha == default)
            {
                throw new HistoricoMantenibilidadInvalidoException("La fecha del registro es obligatoria.");
            }

            if (fecha.Date > DateTime.Today)
            {
                throw new HistoricoMantenibilidadInvalidoException(
                    "La fecha del registro no puede ser posterior al dia de la carga.");
            }
        }

        private static void ValidarEstado(EstadoPanelRegistrado estado, string momento)
        {
            if (!Enum.IsDefined(estado))
            {
                throw new HistoricoMantenibilidadInvalidoException(
                    $"El estado {momento} del panel no es valido.");
            }
        }
    }
}
