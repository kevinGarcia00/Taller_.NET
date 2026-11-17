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
        public bool VolcarEnergia { get; private set; }
        public decimal Potencia { get; private set; }
        public string VidaUtil { get; private set; } = string.Empty;
        public int NodoId { get; private set; }
        public Nodo? Nodo { get; private set; }

        private Panel()
        {
        }

        public Panel(string nombre, EstadoPanel estado, decimal potencia, string vidaUtil, bool volcarEnergia = false)
        {
            ValidarNombre(nombre);
            ValidarEstado(estado);
            ValidarPotencia(potencia);
            ValidarVidaUtil(vidaUtil);

            Nombre = nombre.Trim();
            Estado = estado;
            VolcarEnergia = volcarEnergia;
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

        public void ActualizarDatos(
            string nombre,
            EstadoPanel estado,
            decimal potencia,
            string vidaUtil,
            bool volcarEnergia)
        {
            ValidarNombre(nombre);
            ValidarEstado(estado);
            ValidarPotencia(potencia);
            ValidarVidaUtil(vidaUtil);

            Nombre = nombre.Trim();
            Estado = estado;
            VolcarEnergia = volcarEnergia;
            Potencia = potencia;
            VidaUtil = vidaUtil.Trim();
        }

        public void CambiarEstado(AccionEstadoPanel accion)
        {
            if (!Enum.IsDefined(accion))
            {
                throw new PanelInvalidoException("La accion sobre el panel no es valida.");
            }

            var estadoSolicitado = accion == AccionEstadoPanel.Encender
                ? EstadoPanel.Encendido
                : EstadoPanel.Apagado;

            if (Estado == estadoSolicitado)
            {
                throw new PanelSinCambiosException(
                    $"El panel ya se encuentra en estado {estadoSolicitado}.");
            }

            Estado = estadoSolicitado;
        }

        public void CambiarVolcadoEnergia(bool volcarEnergia)
        {
            if (VolcarEnergia == volcarEnergia)
            {
                throw new VolcadoEnergiaSinCambiosException(
                    $"El panel ya tiene el volcado de energia en {(volcarEnergia ? "habilitado" : "deshabilitado")}.");
            }

            VolcarEnergia = volcarEnergia;
        }

        public decimal CalcularEnergiaAportada()
        {
            return Estado == EstadoPanel.Encendido && VolcarEnergia ? Potencia : 0m;
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
