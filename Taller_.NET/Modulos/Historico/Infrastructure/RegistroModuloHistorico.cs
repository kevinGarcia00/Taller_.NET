using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Taller_.NET.Modulos.Historico.Application;
using Taller_.NET.Modulos.Historico.Application.Dtos;
using Taller_.NET.Modulos.Historico.Application.Servicios;
using Taller_.NET.Modulos.Historico.Application.Validadores;
using Taller_.NET.Modulos.Historico.Domain;
using Taller_.NET.Modulos.Historico.Infrastructure.Persistencia;
using Taller_.NET.Modulos.Historico.Infrastructure.Repositorios;

namespace Taller_.NET.Modulos.Historico.Infrastructure
{
    /// <summary>
    /// Registro de dependencias del modulo Historico. El adaptador del mediador
    /// hacia ParqueSolar se registra en el punto de composicion, fuera del modulo.
    /// </summary>
    public static class RegistroModuloHistorico
    {
        public static IServiceCollection AgregarModuloHistorico(
            this IServiceCollection servicios,
            IConfiguration configuracion)
        {
            servicios.AddDbContext<HistoricoDbContext>(options =>
                options.UseSqlite(configuracion.GetConnectionString("Historico")));

            servicios.AddScoped<IHistoricoDbContext>(proveedor =>
                proveedor.GetRequiredService<HistoricoDbContext>());
            servicios.AddScoped<IHistoricoRepository, HistoricoRepository>();
            servicios.AddScoped<IHistoricoService, HistoricoService>();
            servicios.AddScoped<IValidator<FiltroHistoricoDto>, FiltroHistoricoDtoValidator>();
            servicios.AddHostedService<GeneradorHistoricoBackgroundService>();

            return servicios;
        }
    }
}
