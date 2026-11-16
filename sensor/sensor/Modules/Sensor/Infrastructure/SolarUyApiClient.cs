using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Modules.Sensor.Application.Excepciones;
using Modules.Sensor.Application.Puertos;
using Modules.Sensor.Domain;
using Modules.Sensor.Domain.Excepciones;
using Modules.Sensor.Infrastructure.Modelos;

namespace Modules.Sensor.Infrastructure
{
    public class SolarUyApiClient : ISolarUyApiPort
    {
        private const string RutaInversores = "api/inversor";

        private readonly HttpClient _httpClient;

        public SolarUyApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IReadOnlyList<Inversor>> ObtenerInversoresAsync(
            CancellationToken cancellationToken = default)
        {
            var inversores = await ObtenerAsync<List<InversorApiModel>>(
                RutaInversores,
                cancellationToken);

            return inversores
                .Select(CrearInversor)
                .ToList();
        }

        public async Task<Inversor?> ObtenerInversorAsync(
            int idInversor,
            CancellationToken cancellationToken = default)
        {
            var inversor = await ObtenerAsync<InversorApiModel?>(
                $"{RutaInversores}/{idInversor}",
                cancellationToken);

            return inversor is null ? null : CrearInversor(inversor);
        }

        public async Task<IReadOnlyList<Nodo>> ObtenerNodosAsync(
            int idInversor,
            CancellationToken cancellationToken = default)
        {
            var nodos = await ObtenerAsync<List<NodoApiModel>>(
                $"{RutaInversores}/{idInversor}/nodo",
                cancellationToken);

            return nodos
                .Select(nodo => new Nodo(nodo.Id, nodo.Nombre, nodo.IdInversor))
                .ToList();
        }

        public async Task<IReadOnlyList<Panel>> ObtenerPanelesAsync(
            int idInversor,
            int idNodo,
            CancellationToken cancellationToken = default)
        {
            var paneles = await ObtenerAsync<List<PanelApiModel>>(
                $"{RutaInversores}/{idInversor}/nodo/{idNodo}/panel",
                cancellationToken);

            return paneles
                .Select(CrearPanel)
                .ToList();
        }

        public async Task<Panel?> ObtenerPanelAsync(
            int idInversor,
            int idNodo,
            int idPanel,
            CancellationToken cancellationToken = default)
        {
            var panel = await ObtenerAsync<PanelApiModel?>(
                $"{RutaInversores}/{idInversor}/nodo/{idNodo}/panel/{idPanel}",
                cancellationToken);

            return panel is null ? null : CrearPanel(panel);
        }

        public async Task<Panel?> CambiarEstadoPanelAsync(
            int idInversor,
            int idNodo,
            int idPanel,
            AccionEstadoPanel accion,
            CancellationToken cancellationToken = default)
        {
            var ruta = $"{RutaInversores}/{idInversor}/nodo/{idNodo}/panel/{idPanel}/estado";
            var solicitud = new CambiarEstadoPanelApiModel
            {
                Accion = accion.ToString()
            };

            HttpResponseMessage respuesta;

            try
            {
                respuesta = await _httpClient.PutAsJsonAsync(ruta, solicitud, cancellationToken);
            }
            catch (HttpRequestException excepcion)
            {
                throw new SolarUyNoDisponibleException(
                    $"No se pudo conectar con la API de SolarUY en '{ruta}'.",
                    excepcion);
            }
            catch (TaskCanceledException excepcion) when (!cancellationToken.IsCancellationRequested)
            {
                throw new SolarUyNoDisponibleException(
                    $"La API de SolarUY no respondio a tiempo en '{ruta}'.",
                    excepcion);
            }

            using (respuesta)
            {
                if (respuesta.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new RecursoNoEncontradoException(
                        $"La API de SolarUY no encontro el recurso '{ruta}'.");
                }

                if (respuesta.StatusCode == HttpStatusCode.Conflict)
                {
                    throw new EstadoPanelSinCambiosException(
                        $"La API de SolarUY informo que el panel ya se encuentra en el estado solicitado.");
                }

                if (!respuesta.IsSuccessStatusCode)
                {
                    throw new SolarUyNoDisponibleException(
                        $"La API de SolarUY respondio {(int)respuesta.StatusCode} en '{ruta}'.");
                }

                try
                {
                    var panel = await respuesta.Content.ReadFromJsonAsync<PanelApiModel>(
                        cancellationToken: cancellationToken);

                    return panel is null ? null : CrearPanel(panel);
                }
                catch (JsonException excepcion)
                {
                    throw new SolarUyNoDisponibleException(
                        $"La API de SolarUY devolvio una respuesta invalida en '{ruta}'.",
                        excepcion);
                }
            }
        }

        public async Task<Panel?> CambiarVolcadoEnergiaAsync(
            int idInversor,
            int idNodo,
            int idPanel,
            bool volcarEnergia,
            CancellationToken cancellationToken = default)
        {
            var ruta = $"{RutaInversores}/{idInversor}/nodo/{idNodo}/panel/{idPanel}/volcado";
            var solicitud = new CambiarVolcadoEnergiaApiModel
            {
                VolcarEnergia = volcarEnergia
            };

            HttpResponseMessage respuesta;

            try
            {
                respuesta = await _httpClient.PutAsJsonAsync(ruta, solicitud, cancellationToken);
            }
            catch (HttpRequestException excepcion)
            {
                throw new SolarUyNoDisponibleException(
                    $"No se pudo conectar con la API de SolarUY en '{ruta}'.",
                    excepcion);
            }
            catch (TaskCanceledException excepcion) when (!cancellationToken.IsCancellationRequested)
            {
                throw new SolarUyNoDisponibleException(
                    $"La API de SolarUY no respondio a tiempo en '{ruta}'.",
                    excepcion);
            }

            using (respuesta)
            {
                if (respuesta.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new RecursoNoEncontradoException(
                        $"La API de SolarUY no encontro el recurso '{ruta}'.");
                }

                if (respuesta.StatusCode == HttpStatusCode.Conflict)
                {
                    throw new VolcadoEnergiaSinCambiosException(
                        "La API de SolarUY informo que el panel ya tiene el volcado de energia solicitado.");
                }

                if (!respuesta.IsSuccessStatusCode)
                {
                    throw new SolarUyNoDisponibleException(
                        $"La API de SolarUY respondio {(int)respuesta.StatusCode} en '{ruta}'.");
                }

                try
                {
                    var panel = await respuesta.Content.ReadFromJsonAsync<PanelApiModel>(
                        cancellationToken: cancellationToken);

                    return panel is null ? null : CrearPanel(panel);
                }
                catch (JsonException excepcion)
                {
                    throw new SolarUyNoDisponibleException(
                        $"La API de SolarUY devolvio una respuesta invalida en '{ruta}'.",
                        excepcion);
                }
            }
        }

        private async Task<T> ObtenerAsync<T>(string ruta, CancellationToken cancellationToken)
        {
            HttpResponseMessage respuesta;

            try
            {
                respuesta = await _httpClient.GetAsync(ruta, cancellationToken);
            }
            catch (HttpRequestException excepcion)
            {
                throw new SolarUyNoDisponibleException(
                    $"No se pudo conectar con la API de SolarUY en '{ruta}'.",
                    excepcion);
            }
            catch (TaskCanceledException excepcion) when (!cancellationToken.IsCancellationRequested)
            {
                throw new SolarUyNoDisponibleException(
                    $"La API de SolarUY no respondio a tiempo en '{ruta}'.",
                    excepcion);
            }

            using (respuesta)
            {
                if (respuesta.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new RecursoNoEncontradoException(
                        $"La API de SolarUY no encontro el recurso '{ruta}'.");
                }

                if (!respuesta.IsSuccessStatusCode)
                {
                    throw new SolarUyNoDisponibleException(
                        $"La API de SolarUY respondio {(int)respuesta.StatusCode} en '{ruta}'.");
                }

                try
                {
                    var contenido = await respuesta.Content.ReadFromJsonAsync<T>(
                        cancellationToken: cancellationToken);

                    return contenido ?? throw new SolarUyNoDisponibleException(
                        $"La API de SolarUY devolvio una respuesta vacia en '{ruta}'.");
                }
                catch (JsonException excepcion)
                {
                    throw new SolarUyNoDisponibleException(
                        $"La API de SolarUY devolvio una respuesta invalida en '{ruta}'.",
                        excepcion);
                }
            }
        }

        private static Inversor CrearInversor(InversorApiModel inversor)
        {
            try
            {
                return new Inversor(inversor.Id, inversor.Nombre, inversor.FechaCreacion);
            }
            catch (InversorInvalidoException excepcion)
            {
                throw new SolarUyNoDisponibleException(
                    $"La API de SolarUY devolvio datos invalidos para el inversor {inversor.Id}.",
                    excepcion);
            }
        }

        private static Panel CrearPanel(PanelApiModel panel)
        {
            if (!Enum.TryParse<EstadoPanel>(panel.EstadoPanel, ignoreCase: true, out var estado))
            {
                throw new SolarUyNoDisponibleException(
                    $"La API de SolarUY devolvio un estado de panel invalido: '{panel.EstadoPanel}'.");
            }

            return new Panel(
                panel.Id,
                panel.Nombre,
                panel.FechaCreacion,
                estado,
                panel.Encendido,
                panel.Potencia,
                panel.VidaUtil,
                panel.IdNodo,
                panel.VolcarEnergia);
        }
    }
}
