using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Sensor.Application.Puertos;
using Modules.Sensor.Application.Servicios;

namespace Modules.Sensor.Infrastructure
{
    public static class RegistroModuloSensor
    {
        public static IServiceCollection AgregarModuloSensor(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSignalR();
            services.AddScoped<IConsultaPanelService, ConsultaPanelService>();
            services.AddScoped<ICambiarEstadoPanelService, CambiarEstadoPanelService>();
            services.AddScoped<ICambiarVolcadoEnergiaService, CambiarVolcadoEnergiaService>();
            services.AddScoped<IRecuperarInversorService, RecuperarInversorService>();
            services.AddSingleton<IProveedorTiempoSensor>(new ProveedorTiempoSensor());

            var urlBase = configuration["SolarUy:BaseUrl"] ?? "http://localhost:5284/";
            var tiempoEspera = configuration.GetValue<TimeSpan?>("SolarUy:TiempoEspera")
                               ?? TimeSpan.FromSeconds(10);
            var apiKey = configuration["SolarUy:ApiKey"];

            services.AddHttpClient<ISolarUyApiPort, SolarUyApiClient>(cliente =>
            {
                cliente.BaseAddress = new Uri(urlBase, UriKind.Absolute);
                cliente.Timeout = tiempoEspera;

                if (!string.IsNullOrWhiteSpace(apiKey))
                {
                    cliente.DefaultRequestHeaders.Add("X-Api-Key", apiKey);
                }
            });

            return services;
        }
    }
}
