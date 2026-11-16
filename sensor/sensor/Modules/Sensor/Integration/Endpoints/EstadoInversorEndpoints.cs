using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Sensor.Application.Dtos;
using Modules.Sensor.Application.Excepciones;
using Modules.Sensor.Application.Servicios;
using Modules.Sensor.Domain;

namespace Modules.Sensor.Integration.Endpoints
{
    public static class EstadoInversorEndpoints
    {
        public static IEndpointRouteBuilder MapEstadoInversorEndpoints(this IEndpointRouteBuilder app)
        {
            var grupo = app.MapGroup("/inversor");
            var grupoApi = app.MapGroup("/api/inversor");

            grupoApi.MapGet("/{idInversor:int}", RecuperarInversorAsync);

            grupo.MapGet("/", ObtenerInversoresAsync);
            grupo.MapGet("/{idInversor:int}/nodo", ObtenerNodosAsync);
            grupo.MapGet("/{idInversor:int}/nodo/{idNodo:int}/panel", ObtenerPanelesAsync);
            grupo.MapGet("/{idInversor:int}/nodo/{idNodo:int}/panel/{idPanel:int}", ObtenerPanelAsync);
            grupo.MapPut("/{idInversor:int}/nodo/{idNodo:int}/panel/{idPanel:int}/estado", CambiarEstadoPanelAsync);
            grupo.MapPut("/{idInversor:int}/nodo/{idNodo:int}/panel/{idPanel:int}/volcado", CambiarVolcadoEnergiaAsync);

            return app;
        }

        private static async Task<IResult> ObtenerInversoresAsync(
            IConsultaPanelService servicio,
            CancellationToken cancellationToken)
        {
            try
            {
                var inversores = await servicio.ObtenerInversoresAsync(cancellationToken);
                return Results.Ok(inversores);
            }
            catch (SolarUyNoDisponibleException excepcion)
            {
                return CrearError(StatusCodes.Status502BadGateway, excepcion.Message);
            }
        }

        private static async Task<IResult> RecuperarInversorAsync(
            int idInversor,
            IRecuperarInversorService servicio,
            CancellationToken cancellationToken)
        {
            try
            {
                var inversor = await servicio.RecuperarInversorAsync(
                    idInversor,
                    cancellationToken);

                return Results.Ok(inversor);
            }
            catch (ArgumentOutOfRangeException excepcion)
            {
                return CrearError(StatusCodes.Status400BadRequest, excepcion.Message);
            }
            catch (RecursoNoEncontradoException excepcion)
            {
                return CrearError(StatusCodes.Status404NotFound, excepcion.Message);
            }
            catch (SolarUyNoDisponibleException excepcion)
            {
                return CrearError(StatusCodes.Status502BadGateway, excepcion.Message);
            }
        }

        private static async Task<IResult> ObtenerNodosAsync(
            int idInversor,
            IConsultaPanelService servicio,
            CancellationToken cancellationToken)
        {
            try
            {
                var nodos = await servicio.ObtenerNodosInversorAsync(idInversor, cancellationToken);
                return Results.Ok(nodos);
            }
            catch (ArgumentOutOfRangeException excepcion)
            {
                return CrearError(StatusCodes.Status400BadRequest, excepcion.Message);
            }
            catch (RecursoNoEncontradoException excepcion)
            {
                return CrearError(StatusCodes.Status404NotFound, excepcion.Message);
            }
            catch (SolarUyNoDisponibleException excepcion)
            {
                return CrearError(StatusCodes.Status502BadGateway, excepcion.Message);
            }
        }

        private static async Task<IResult> ObtenerPanelesAsync(
            int idInversor,
            int idNodo,
            IConsultaPanelService servicio,
            CancellationToken cancellationToken)
        {
            try
            {
                var paneles = await servicio.ObtenerPanelesNodoInversorAsync(
                    idInversor,
                    idNodo,
                    cancellationToken);

                return Results.Ok(paneles);
            }
            catch (ArgumentOutOfRangeException excepcion)
            {
                return CrearError(StatusCodes.Status400BadRequest, excepcion.Message);
            }
            catch (RecursoNoEncontradoException excepcion)
            {
                return CrearError(StatusCodes.Status404NotFound, excepcion.Message);
            }
            catch (SolarUyNoDisponibleException excepcion)
            {
                return CrearError(StatusCodes.Status502BadGateway, excepcion.Message);
            }
        }

        private static async Task<IResult> ObtenerPanelAsync(
            int idInversor,
            int idNodo,
            int idPanel,
            IConsultaPanelService servicio,
            CancellationToken cancellationToken)
        {
            try
            {
                var panel = await servicio.ObtenerPanelAsync(
                    idInversor,
                    idNodo,
                    idPanel,
                    cancellationToken);

                return Results.Ok(panel);
            }
            catch (ArgumentOutOfRangeException excepcion)
            {
                return CrearError(StatusCodes.Status400BadRequest, excepcion.Message);
            }
            catch (RecursoNoEncontradoException excepcion)
            {
                return CrearError(StatusCodes.Status404NotFound, excepcion.Message);
            }
            catch (SolarUyNoDisponibleException excepcion)
            {
                return CrearError(StatusCodes.Status502BadGateway, excepcion.Message);
            }
        }

        private static async Task<IResult> CambiarEstadoPanelAsync(
            int idInversor,
            int idNodo,
            int idPanel,
            CambiarEstadoPanelSolicitudDto solicitud,
            ICambiarEstadoPanelService servicio,
            CancellationToken cancellationToken)
        {
            if (!Enum.TryParse<AccionEstadoPanel>(solicitud.Accion, ignoreCase: true, out var accion) ||
                !Enum.IsDefined(accion))
            {
                return CrearError(
                    StatusCodes.Status400BadRequest,
                    "La accion debe ser 'Encender' o 'Apagar'.");
            }

            try
            {
                var panel = await servicio.CambiarEstadoPanelAsync(
                    new CambiarEstadoPanelDto
                    {
                        IdInversor = idInversor,
                        IdNodo = idNodo,
                        IdPanel = idPanel,
                        Accion = accion
                    },
                    cancellationToken);

                return Results.Ok(panel);
            }
            catch (ArgumentOutOfRangeException excepcion)
            {
                return CrearError(StatusCodes.Status400BadRequest, excepcion.Message);
            }
            catch (RecursoNoEncontradoException excepcion)
            {
                return CrearError(StatusCodes.Status404NotFound, excepcion.Message);
            }
            catch (EstadoPanelSinCambiosException excepcion)
            {
                return CrearError(StatusCodes.Status409Conflict, excepcion.Message);
            }
            catch (SolarUyNoDisponibleException excepcion)
            {
                return CrearError(StatusCodes.Status502BadGateway, excepcion.Message);
            }
        }

        private static async Task<IResult> CambiarVolcadoEnergiaAsync(
            int idInversor,
            int idNodo,
            int idPanel,
            CambiarVolcadoEnergiaSolicitudDto solicitud,
            ICambiarVolcadoEnergiaService servicio,
            CancellationToken cancellationToken)
        {
            if (solicitud.VolcarEnergia is null)
            {
                return CrearError(
                    StatusCodes.Status400BadRequest,
                    "El cuerpo debe incluir el valor booleano 'volcarEnergia'.");
            }

            try
            {
                var panel = await servicio.CambiarVolcadoEnergiaAsync(
                    new CambiarVolcadoEnergiaDto
                    {
                        IdInversor = idInversor,
                        IdNodo = idNodo,
                        IdPanel = idPanel,
                        VolcarEnergia = solicitud.VolcarEnergia.Value
                    },
                    cancellationToken);

                return Results.Ok(panel);
            }
            catch (ArgumentOutOfRangeException excepcion)
            {
                return CrearError(StatusCodes.Status400BadRequest, excepcion.Message);
            }
            catch (RecursoNoEncontradoException excepcion)
            {
                return CrearError(StatusCodes.Status404NotFound, excepcion.Message);
            }
            catch (VolcadoEnergiaSinCambiosException excepcion)
            {
                return CrearError(StatusCodes.Status409Conflict, excepcion.Message);
            }
            catch (SolarUyNoDisponibleException excepcion)
            {
                return CrearError(StatusCodes.Status502BadGateway, excepcion.Message);
            }
        }

        private static IResult CrearError(int codigoEstado, string mensaje)
        {
            return Results.Json(new { mensaje }, statusCode: codigoEstado);
        }
    }
}
