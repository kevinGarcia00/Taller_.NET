using System;

namespace Taller_.NET.Modulos.Historico.Application.Excepciones
{
    public class PanelFueraDeContextoException : Exception
    {
        public PanelFueraDeContextoException(int panelId)
            : base($"No se encontro el panel {panelId} en el nodo y el parque seleccionados.")
        {
        }
    }
}
