using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Taller_.NET.Modulos.Historico.Application;
using Taller_.NET.Modulos.Historico.Application.Dtos;
using Taller_.NET.Modulos.Historico.Application.Puertos;
using Taller_.NET.Modulos.Historico.Application.Servicios;
using Taller_.NET.Modulos.Historico.Application.Validadores;
using Taller_.NET.Modulos.Historico.Domain;
using Taller_.NET.Modulos.Historico.Infrastructure.Persistencia;
using Taller_.NET.Modulos.Historico.Infrastructure.Repositorios;
using Taller_.NET.Modulos.Historico.Infrastructure.Seguridad;

namespace Taller_.NET.Modulos.Historico.Infrastructure
{
    public static class RegistroModuloHistorico
    {
        public static IServiceCollection AgregarModuloHistorico(
            this IServiceCollection servicios,
            IConfiguration configuracion)
        {
            var conexion = configuracion.GetConnectionString("Historico")
                ?? configuracion.GetConnectionString("ParqueSolar");

            servicios.AddDbContext<HistoricoDbContext>(options =>
                options.UseSqlite(conexion));

            servicios.AddScoped<IHistoricoDbContext>(proveedor =>
                proveedor.GetRequiredService<HistoricoDbContext>());
            servicios.AddScoped<IHistoricoMantenibilidadRepository, HistoricoMantenibilidadRepository>();
            servicios.AddScoped<IHistoricoMantenibilidadService, HistoricoMantenibilidadService>();
            servicios.AddScoped<IUsuarioActualHistorico, UsuarioActualHistorico>();
            servicios.AddScoped<IValidator<CrearHistoricoMantenibilidadDto>, CrearHistoricoMantenibilidadDtoValidator>();

            return servicios;
        }
    }
}
