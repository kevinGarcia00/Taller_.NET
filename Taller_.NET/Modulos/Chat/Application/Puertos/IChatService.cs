namespace Taller_.NET.Modulos.Chat.Application.Puertos
{
    /*
     Define el contrato de la conversación para que la interfaz de usuario no dependa
     directamente de Antigravity, de procesos del sistema operativo ni de MCP.
    */
    public interface IChatService
    {
        Task<string> ResponderAsync(string pregunta, CancellationToken cancellationToken = default);
    }
}
