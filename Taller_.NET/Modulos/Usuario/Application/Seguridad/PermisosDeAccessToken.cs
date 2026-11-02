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
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                return Array.Empty<string>();
            }

            var segmentos = accessToken.Split('.');

            if (segmentos.Length < 2)
            {
                return Array.Empty<string>();
            }

            try
            {
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
        }

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
