using System.Security.Claims;
using System.Text.Encodings.Web;
using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Radzen;
using Taller_.NET.Components;
using Taller_.NET.Modulos.ParqueSolar.Application;
using Taller_.NET.Modulos.ParqueSolar.Application.Dtos;
using Taller_.NET.Modulos.ParqueSolar.Application.Puertos;
using Taller_.NET.Modulos.ParqueSolar.Application.Servicios;
using Taller_.NET.Modulos.ParqueSolar.Application.Validadores;
using Taller_.NET.Modulos.ParqueSolar.Domain;
using Taller_.NET.Modulos.ParqueSolar.Infrastructure.Persistencia;
using Taller_.NET.Modulos.ParqueSolar.Infrastructure.Repositorios;
using Taller_.NET.Modulos.ParqueSolar.Infrastructure.Seguridad;

var builder = WebApplication.CreateBuilder(args);

var permitirUsuariosAnonimos = builder.Configuration.GetValue<bool>("Auth0:PermitirUsuariosAnonimos");

if (permitirUsuariosAnonimos)
{
    builder.Services.AddAuthentication(ModoDesarrolloAnonimo)
        .AddScheme<AuthenticationSchemeOptions, AutenticacionAnonimaHandler>(
            ModoDesarrolloAnonimo,
            _ => { });
    builder.Services.AddAuthorization(options =>
        options.DefaultPolicy = new AuthorizationPolicyBuilder(ModoDesarrolloAnonimo)
            .RequireAssertion(_ => true)
            .Build());
}
else
{
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Auth0:Authority"];
        options.Audience = builder.Configuration["Auth0:Audience"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
            RoleClaimType = builder.Configuration["Auth0:RolesClaim"] ?? "https://solaryu.com/roles"
        };
    });

builder.Services.AddAuthorization();
}
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddRadzenComponents();

builder.Services.AddDbContext<ParqueDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("ParqueSolar")));

builder.Services.AddScoped<IParqueDbContext>(proveedor => proveedor.GetRequiredService<ParqueDbContext>());
builder.Services.AddScoped<IParqueRepository, ParqueRepository>();
builder.Services.AddScoped<IParqueService, ParqueService>();
builder.Services.AddScoped<IUsuarioActualProvider, UsuarioActualProvider>();
builder.Services.AddScoped<IValidator<CrearParqueDto>, CrearParqueDtoValidator>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

public partial class Program
{
    private const string ModoDesarrolloAnonimo = "DesarrolloAnonimo";
}

public class AutenticacionAnonimaHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public AutenticacionAnonimaHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var identidad = new ClaimsIdentity(
            new[]
            {
                new Claim(ClaimTypes.Name, "Usuario de prueba"),
                new Claim(ClaimTypes.Role, "Administrador")
            },
            Scheme.Name);

        var principal = new ClaimsPrincipal(identidad);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
