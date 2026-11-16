using System;
using System.Threading;
using System.Threading.Tasks;
using Modules.Sensor.Application.Dtos;
using Modules.Sensor.Application.Excepciones;
using Modules.Sensor.Application.Puertos;
using Modules.Sensor.Domain;

namespace Modules.Sensor.Application.Servicios
{
    public class CambiarEstadoPanelService : ICambiarEstadoPanelService
    {
        private readonly ISolarUyApiPort _solarUyApiPort;

        public CambiarEstadoPanelService(ISolarUyApiPort solarUyApiPort)
        {
            _solarUyApiPort = solarUyApiPort;
        }

        public async Task<PanelDetalleDto> CambiarEstadoPanelAsync(
            CambiarEstadoPanelDto dto,
            CancellationToken cancellationToken = default)
        {
            ValidarIdentificador(dto.IdInversor, nameof(dto.IdInversor));
            ValidarIdentificador(dto.IdNodo, nameof(dto.IdNodo));
            ValidarIdentificador(dto.IdPanel, nameof(dto.IdPanel));

            if (!Enum.IsDefined(dto.Accion))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(dto.Accion),
                    "La accion sobre el panel no es valida.");
            }

            var panel = await _solarUyApiPort.ObtenerPanelAsync(
                dto.IdInversor,
                dto.IdNodo,
                dto.IdPanel,
                cancellationToken);

            if (panel is null || panel.IdNodo != dto.IdNodo)
            {
                throw new RecursoNoEncontradoException(
                    $"No se encontro el panel {dto.IdPanel} en el nodo {dto.IdNodo} del inversor {dto.IdInversor}.");
            }

            if (!panel.PuedeCambiarA(dto.Accion))
            {
                throw new EstadoPanelSinCambiosException(
                    $"El panel {dto.IdPanel} ya se encuentra en el estado solicitado.");
            }

            var panelActualizado = await _solarUyApiPort.CambiarEstadoPanelAsync(
                dto.IdInversor,
                dto.IdNodo,
                dto.IdPanel,
                dto.Accion,
                cancellationToken);

            if (panelActualizado is null)
            {
                throw new RecursoNoEncontradoException(
                    $"No se encontro el panel {dto.IdPanel} en el nodo {dto.IdNodo} del inversor {dto.IdInversor}.");
            }

            return new PanelDetalleDto
            {
                Id = panelActualizado.Id,
                Nombre = panelActualizado.Nombre,
                FechaCreacion = panelActualizado.FechaCreacion,
                EstadoPanel = panelActualizado.Estado.ToString(),
                Encendido = panelActualizado.Encendido,
                VolcarEnergia = panelActualizado.VolcarEnergia,
                Potencia = panelActualizado.Potencia,
                VidaUtil = panelActualizado.VidaUtil,
                IdInversor = dto.IdInversor,
                IdNodo = dto.IdNodo
            };
        }

        private static void ValidarIdentificador(int valor, string nombre)
        {
            if (valor <= 0)
            {
                throw new ArgumentOutOfRangeException(nombre, "El identificador debe ser mayor a cero.");
            }
        }
    }
}
