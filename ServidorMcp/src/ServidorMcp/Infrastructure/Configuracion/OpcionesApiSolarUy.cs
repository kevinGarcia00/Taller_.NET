namespace Infrastructure.Configuracion;

public sealed class OpcionesApiSolarUy
{
    public Uri BaseUrl { get; }

    public OpcionesApiSolarUy(Uri baseUrl)
    {
        ArgumentNullException.ThrowIfNull(baseUrl);
        BaseUrl = baseUrl;
    }
}
