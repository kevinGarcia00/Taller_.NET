using System.Security.Claims;
using System.Text.Encodings.Web;
using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
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
using Taller_.NET.Modulos.ParqueSolar.Infrastructure.Mediadores;
using Taller_.NET.Modulos.ParqueSolar.Infrastructure.Persistencia;
using Taller_.NET.Modulos.ParqueSolar.Infrastructure.Repositorios;
using Taller_.NET.Modulos.ParqueSolar.Infrastructure.Seguridad;
using Taller_.NET.Modulos.Usuario.Application;
using Taller_.NET.Modulos.Usuario.Application.Dtos;
using Taller_.NET.Modulos.Usuario.Application.Servicios;
using Taller_.NET.Modulos.Usuario.Application.Validadores;
using Taller_.NET.Modulos.Usuario.Infrastructure.Persistencia;
using Taller_.NET.Modulos.Usuario.Infrastructure.Repositorios;
// El modulo ParqueSolar y el modulo Usuario declaran tipos con el mismo nombre,
// por lo que los del modulo Usuario se referencian con alias.
using IUsuarioActualProviderUsuario = Taller_.NET.Modulos.Usuario.Application.Puertos.IUsuarioActualProvider;
using IUsuarioRepositoryUsuario = Taller_.NET.Modulos.Usuario.Application.Puertos.IUsuarioRepository;
using UsuarioActualProviderUsuario = Taller_.NET.Modulos.Usuario.Infrastructure.Seguridad.UsuarioActualProvider;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                               ForwardedHeaders.XForwardedProto |
                               ForwardedHeaders.XForwardedHost;

    // Desarrollo con tuneles (ngrok) y proxys locales.
    options.KnownIPNetworks.Clear();
    options.KnownProxies.Clear();
});

var permitirUsuariosAnonimos = builder.Configuration.GetValue<bool>("Auth0:PermitirUsuariosAnonimos");

if (!permitirUsuariosAnonimos)
{
    var configuracionFaltante = new List<string>();

    if (string.IsNullOrWhiteSpace(builder.Configuration["Auth0:Authority"]))
    {
        configuracionFaltante.Add("Auth0:Authority");
    }

    if (string.IsNullOrWhiteSpace(builder.Configuration["Auth0:ClientId"]))
    {
        configuracionFaltante.Add("Auth0:ClientId");
    }

    if (configuracionFaltante.Count > 0)
    {
        throw new InvalidOperationException(
            $"Falta la configuracion de Auth0: {string.Join(", ", configuracionFaltante)}. " +
            "El ClientId es el de una aplicacion (Regular Web Application) del tenant, no el identificador de la API. " +
            "Definalo con 'dotnet user-secrets set \"Auth0:ClientId\" \"...\"' o como variable de entorno " +
            "(Auth0__ClientId) o levante la app en modo de desarrollo con Auth0__PermitirUsuariosAnonimos=true.");
    }
}

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
    // La interfaz web usa cookie + OpenID Connect contra Auth0 (login en el navegador).
    // El esquema "Bearer" (JwtBearer) queda disponible para las APIs que consumen
    // el token de Auth0 directamente.
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    })
    .AddCookie(options =>
    {
        options.LoginPath = "/login";
        options.LogoutPath = "/logout";
        options.AccessDeniedPath = "/login";
    })
    .AddOpenIdConnect(options =>
    {
        var origenPublico = builder.Configuration["Auth0:PublicOrigin"]?.TrimEnd('/');

        options.Authority = builder.Configuration["Auth0:Authority"];

        // Client ID de la aplicacion de Auth0 (Regular Web Application). No puede ser
        // el identificador de la API: Auth0 responde "Unknown client" para ese caso.
        options.ClientId = builder.Configuration["Auth0:ClientId"];

        // El ClientSecret es opcional: si no se define, el flujo usa PKCE sin secreto.
        options.ClientSecret = builder.Configuration["Auth0:ClientSecret"];

        options.CallbackPath = "/signin-oidc";
        options.ResponseType = "code";
        options.UsePkce = true;
        options.SaveTokens = true;
        options.GetClaimsFromUserInfoEndpoint = true;
        options.SignedOutCallbackPath = "/signout-callback-oidc";

        // Scopes minimos sin configuraciones complejas
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("email");

        options.Events = new OpenIdConnectEvents
        {
            OnRedirectToIdentityProvider = context =>
            {
                if (!string.IsNullOrWhiteSpace(origenPublico))
                {
                    context.ProtocolMessage.RedirectUri = $"{origenPublico}{options.CallbackPath}";
                }

                return Task.CompletedTask;
            },
            OnRedirectToIdentityProviderForSignOut = context =>
            {
                if (!string.IsNullOrWhiteSpace(origenPublico))
                {
                    context.ProtocolMessage.PostLogoutRedirectUri =
                        $"{origenPublico}{options.SignedOutCallbackPath}";
                }

                return Task.CompletedTask;
            }
        };

        // Auth0 entrega los roles en el claim con namespace (agregado por una Action).
        options.TokenValidationParameters.RoleClaimType = builder.Configuration["Auth0:RolesClaim"] ?? "https://solaryu.com/roles";
        options.TokenValidationParameters.NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
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
}

builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAntiforgery();

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
builder.Services.AddScoped<IMediadorParqueNodo, MediadorParqueNodo>();
builder.Services.AddScoped<INodoRepository, NodoRepository>();
builder.Services.AddScoped<INodoService, NodoService>();
builder.Services.AddScoped<IMediadorNodoParque, MediadorNodoParque>();
builder.Services.AddScoped<IMediadorNodoInversor, MediadorNodoInversor>();
builder.Services.AddScoped<IUsuarioActualProvider, UsuarioActualProvider>();
builder.Services.AddScoped<IValidator<CrearParqueDto>, CrearParqueDtoValidator>();
builder.Services.AddScoped<IValidator<ModificarParqueDto>, ModificarParqueDtoValidator>();
builder.Services.AddScoped<IValidator<CrearNodoDto>, CrearNodoDtoValidator>();
builder.Services.AddScoped<IValidator<EditarNodoDto>, EditarNodoDtoValidator>();

builder.Services.AddDbContext<UsuarioDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Usuario")));

builder.Services.AddScoped<IUsuarioDbContext>(proveedor => proveedor.GetRequiredService<UsuarioDbContext>());
builder.Services.AddScoped<IUsuarioRepositoryUsuario, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IUsuarioActualProviderUsuario, UsuarioActualProviderUsuario>();
builder.Services.AddScoped<IValidator<CrearUsuarioDto>, CrearUsuarioDtoValidator>();
builder.Services.AddScoped<IValidator<ModificarUsuarioDto>, ModificarUsuarioDtoValidator>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseForwardedHeaders();
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