namespace Taller_.NET.Modulos.Historico.Application.Excepciones
{
    public class PanelNoEncontradoException : Exception
    {
        public PanelNoEncontradoException(int panelId)
            : base($"No se encontro un panel con el identificador {panelId} en el nodo y parque seleccionados.")
        {
        }
    }
}
