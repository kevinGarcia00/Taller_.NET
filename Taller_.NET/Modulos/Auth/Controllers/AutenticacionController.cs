using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;

namespace Taller_.NET.Modulos.Auth.Controllers
{
    public class AutenticacionController : Controller
    {
        [HttpGet("/login")]
        public IActionResult Login(string? returnUrl = null)
        {
            var propiedades = new AuthenticationProperties
            {
                RedirectUri = returnUrl ?? "/"
            };

            return Challenge(propiedades, OpenIdConnectDefaults.AuthenticationScheme);
        }

        [HttpPost("/logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var antiforgery = HttpContext.RequestServices.GetRequiredService<IAntiforgery>();
                await antiforgery.ValidateRequestAsync(HttpContext);
            }
            catch (AntiforgeryValidationException)
            {
                return BadRequest("El token de verificacion no es valido.");
            }

            var proveedor = HttpContext.RequestServices.GetRequiredService<IAuthenticationSchemeProvider>();
            var handlers = HttpContext.RequestServices.GetRequiredService<IAuthenticationHandlerProvider>();
            var esquemas = new List<string>();

            foreach (var nombre in new[] { CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme })
            {
                if (proveedor.GetSchemeAsync(nombre) is null)
                {
                    continue;
                }

                var handler = await handlers.GetHandlerAsync(HttpContext, nombre);

                if (handler is IAuthenticationSignOutHandler)
                {
                    esquemas.Add(nombre);
                }
            }

            if (esquemas.Count == 0)
            {
                // Modo de desarrollo: no hay sesion que cerrar.
                return Redirect("/");
            }

            var propiedades = new AuthenticationProperties
            {
                RedirectUri = "/"
            };

            return SignOut(propiedades, esquemas.ToArray());
        }
    }
}
