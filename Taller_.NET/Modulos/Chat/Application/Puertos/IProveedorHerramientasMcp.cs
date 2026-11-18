namespace Taller_.NET.Modulos.Chat.Application.Puertos
{
    
    public interface IProveedorHerramientasMcp
    {
        /*
         Consulta los nombres de las herramientas publicadas para limitar a ellas
         las capacidades que Antigravity podrá utilizar en la respuesta.
         */
        Task<IReadOnlyList<string>> ObtenerNombresHerramientasAsync(
            CancellationToken cancellationToken = default);
    }
}
