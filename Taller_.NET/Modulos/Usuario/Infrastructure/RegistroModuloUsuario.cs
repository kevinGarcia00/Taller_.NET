using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Taller_.NET.Modulos.Usuario.Application;
using Taller_.NET.Modulos.Usuario.Application.Dtos;
using Taller_.NET.Modulos.Usuario.Application.Puertos;
using Taller_.NET.Modulos.Usuario.Application.Seguridad;
using Taller_.NET.Modulos.Usuario.Application.Servicios;
using Taller_.NET.Modulos.Usuario.Application.Validadores;
using Taller_.NET.Modulos.Usuario.Infrastructure.Auth0;
using Taller_.NET.Modulos.Usuario.Infrastructure.Persistencia;
using Taller_.NET.Modulos.Usuario.Infrastructure.Repositorios;
using Taller_.NET.Modulos.Usuario.Infrastructure.Seguridad;

namespace Taller_.NET.Modulos.Usuario.Infrastructure
{
    /// <summary>
    /// Registro de dependencias del modulo Usuario. Mantiene el cableado del modulo
    /// encapsulado para que el punto de composicion solo invoque este metodo.
    /// </summary>
    public static class RegistroModuloUsuario
    {
        public static IServiceCollection AgregarModuloUsuario(
            this IServiceCollection servicios,
            IConfiguration configuracion)
        {
            servicios.AddDbContext<UsuarioDbContext>(options =>
                options.UseSqlite(configuracion.GetConnectionString("Usuario")));

            servicios.AddScoped<IUsuarioDbContext>(proveedor => proveedor.GetRequiredService<UsuarioDbContext>());
            servicios.AddScoped<IUsuarioRepository, UsuarioRepository>();
            servicios.AddScoped<IUsuarioService, UsuarioService>();
            servicios.AddScoped<IUsuarioAutenticacionService, UsuarioAutenticacionService>();
            servicios.AddScoped<IUsuarioActualProvider, UsuarioActualProvider>();
            servicios.AddScoped<InicializadorDatosUsuario>();

            servicios.AddScoped<IValidator<CrearUsuarioDto>, CrearUsuarioDtoValidator>();
            servicios.AddScoped<IValidator<ModificarUsuarioDto>, ModificarUsuarioDtoValidator>();

            AgregarPoliticasDeAutorizacion(servicios);

            if (configuracion.GetValue<bool>("Auth0:PermitirUsuariosAnonimos"))
            {
                servicios.AddScoped<IAuth0AdministracionProvider, Auth0AdministracionProviderAnonimo>();
            }
            else
            {
                servicios.AddHttpClient<IAuth0AdministracionProvider, Auth0AdministracionProvider>(cliente =>
                {
                    cliente.Timeout = TimeSpan.FromSeconds(30);
                });
            }

            return servicios;
        }

        /// <summary>
        /// Ejecuta las tareas de inicializacion del modulo (por ejemplo, garantizar
        /// la existencia del administrador inicial) dentro de un alcance propio.
        /// </summary>
        public static async Task InicializarModuloUsuarioAsync(
            this IServiceProvider proveedor,
            CancellationToken cancellationToken = default)
        {
            await using var alcance = proveedor.CreateAsyncScope();

            var inicializador = alcance.ServiceProvider.GetRequiredService<InicializadorDatosUsuario>();

            await inicializador.AsegurarAdministradorInicialAsync(cancellationToken);
        }

        private static void AgregarPoliticasDeAutorizacion(IServiceCollection servicios)
        {
            servicios.AddAuthorization(opciones =>
            {
                opciones.AddPolicy(
                    PermisosUsuarios.PoliticaLeer,
                    politica => politica
                        .RequireAuthenticatedUser()
                        .RequireClaim(PermisosUsuarios.TipoClaim, PermisosUsuarios.Leer));

                opciones.AddPolicy(
                    PermisosUsuarios.PoliticaCrear,
                    politica => politica
                        .RequireAuthenticatedUser()
                        .RequireClaim(PermisosUsuarios.TipoClaim, PermisosUsuarios.Crear));

                opciones.AddPolicy(
                    PermisosUsuarios.PoliticaActualizar,
                    politica => politica
                        .RequireAuthenticatedUser()
                        .RequireClaim(PermisosUsuarios.TipoClaim, PermisosUsuarios.Actualizar));

                opciones.AddPolicy(
                    PermisosUsuarios.PoliticaEliminar,
                    politica => politica
                        .RequireAuthenticatedUser()
                        .RequireClaim(PermisosUsuarios.TipoClaim, PermisosUsuarios.Eliminar));
            });
        }
    }
}
