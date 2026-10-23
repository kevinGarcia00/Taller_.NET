namespace Taller_.NET.Modulos.Usuario.Application.Puertos
{
    public interface IUsuarioActualProvider
    {
        Task<bool> EsAdministradorAsync(CancellationToken cancellationToken = default);
    }
}
