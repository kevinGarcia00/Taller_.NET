namespace Infrastructure.Configuracion;

public sealed class OpcionesBaseDatosSolarUy
{
    public string RutaUsuarios { get; }

    public OpcionesBaseDatosSolarUy(string rutaUsuarios)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(rutaUsuarios);
        RutaUsuarios = rutaUsuarios;
    }
}