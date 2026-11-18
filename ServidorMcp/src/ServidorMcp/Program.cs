using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using Application.Puertos;
using Application.Servicios;
using Infrastructure.Configuracion;
using Infrastructure.Repositorios;
using Presentacion.Herramientas;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ContentRootPath = AppContext.BaseDirectory
});

builder.Logging.AddConsole(opciones =>
{
    opciones.LogToStandardErrorThreshold = LogLevel.Trace;
});

var urlBase = builder.Configuration["ApiSolarUy:BaseUrl"];

if (!Uri.TryCreate(urlBase, UriKind.Absolute, out var urlBaseApi))
{
    throw new InvalidOperationException("Falta la URL base valida 'ApiSolarUy:BaseUrl' de la API de SolarUY.");
}

var opcionesApi = new OpcionesApiSolarUy(urlBaseApi);
var rutaUsuariosConfigurada = builder.Configuration["DatosSolarUy:RutaUsuarios"];

if (string.IsNullOrWhiteSpace(rutaUsuariosConfigurada))
{
    throw new InvalidOperationException("Falta la ruta de SQLite 'DatosSolarUy:RutaUsuarios'.");
}

var rutaBaseDatosUsuarios = Path.GetFullPath(rutaUsuariosConfigurada, builder.Environment.ContentRootPath);
var opcionesBaseDatosSolarUy = new OpcionesBaseDatosSolarUy(rutaBaseDatosUsuarios);

builder.Services.AddSingleton(opcionesApi);
builder.Services.AddSingleton(opcionesBaseDatosSolarUy);
builder.Services.AddHttpClient<IParqueSolarConsultaRepository, ParqueSolarHttpRepository>(cliente =>
{
    cliente.BaseAddress = opcionesApi.BaseUrl;
});
builder.Services.AddScoped<IParqueConsultaService, ParqueConsultaService>();

builder.Services.AddScoped<IUsuarioConsultaRepository, UsuarioSqliteRepository>();
builder.Services.AddScoped<IUsuarioConsultaService, UsuarioConsultaService>();

builder.Services.AddMcpServer()
    .WithHttpTransport()
    .WithTools<HerramientaParque>()
    .WithTools<HerramientaUsuario>();

var urlsMcp = builder.Configuration["Mcp:Urls"];

if (!string.IsNullOrWhiteSpace(urlsMcp))
{
    builder.WebHost.UseUrls(urlsMcp);
}

var app = builder.Build();

// Antes de exponer el endpoint, confirmo que las unicas fuentes de datos sean herramientas MCP.
ValidadorHerramientasMcp.Validar(app.Services, app.Logger);

app.MapMcp("/mcp");

await app.RunAsync();
