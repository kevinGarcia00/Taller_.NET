using System.Text;
using System.Text.Json;

namespace Taller_.NET.Modulos.Usuario.Application.Seguridad
{
    /// <summary>
    /// Extrae los permisos incluidos en el claim "permissions" de un Access Token
    /// JWT emitido por Auth0. Si el token no es JWT o no contiene el claim, devuelve
    /// una coleccion vacia.
    /// </summary>
    public static class PermisosDeAccessToken
    {

        public static IReadOnlyList<string> Obtener(string? accessToken)
        {
            //si el access token me llega null, devuelvo lista vacia
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                return Array.Empty<string>();
            }

            // separo en segmentos el token
            var segmentos = accessToken.Split('.');

            // controlo que sea un token valido
            // para que sea valido tiene que tener 2 segmentos
            if (segmentos.Length < 2)
            {
                return Array.Empty<string>();
            }

            try
            {   
                //decodificamos para tener el JSON
                var payload = DecodificarBase64Url(segmentos[1]);

                using var documento = JsonDocument.Parse(payload);

                if (!documento.RootElement.TryGetProperty(PermisosUsuarios.TipoClaim, out var permisos) ||
                    permisos.ValueKind != JsonValueKind.Array)
                {
                    return Array.Empty<string>();
                }

                return permisos
                    .EnumerateArray()
                    .Select(permiso => permiso.GetString())
                    .Where(permiso => !string.IsNullOrWhiteSpace(permiso))
                    .Select(permiso => permiso!)
                    .Distinct(StringComparer.Ordinal)
                    .ToList();
            }
            catch (FormatException)
            {
                return Array.Empty<string>();
            }
            catch (JsonException)
            {
                return Array.Empty<string>();
            }

        /*
        JSON esperado
            {
                "iss": "https://dev-615iep3t3nwn45qm.us.auth0.com/",
                "sub": "auth0|64f1a2b3c4d5e6f7a8b9c0d1",
                "aud": ["https://api.misolaruy.com", "https://dev-615iep3t3nwn45qm.us.auth0.com/userinfo"],
                "iat": 1731800000,
                "exp": 1731836000,
                "scope": "openid profile email",
                "permissions": [
                    "users:read",
                    "users:create",
                    "users:update",
                    "users:delete"
                ]
            }
        */
        }

        // auxiliar para decodificar el json
        private static string DecodificarBase64Url(string valor)
        {
            var normalizado = valor.Replace('-', '+').Replace('_', '/');

            switch (normalizado.Length % 4)
            {
                case 2:
                    normalizado += "==";
                    break;
                case 3:
                    normalizado += "=";
                    break;
            }

            return Encoding.UTF8.GetString(Convert.FromBase64String(normalizado));
        }
    }
}
