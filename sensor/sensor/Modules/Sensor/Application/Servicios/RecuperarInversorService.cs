using System;
using System.Threading;
using System.Threading.Tasks;
using Modules.Sensor.Application.Dtos;
using Modules.Sensor.Application.Excepciones;
using Modules.Sensor.Application.Puertos;

namespace Modules.Sensor.Application.Servicios
{
    public class RecuperarInversorService : IRecuperarInversorService
    {
        private readonly ISolarUyApiPort _solarUyApiPort;
        private readonly IProveedorTiempoSensor _proveedorTiempoSensor;

        public RecuperarInversorService(
            ISolarUyApiPort solarUyApiPort,
            IProveedorTiempoSensor proveedorTiempoSensor)
        {
            _solarUyApiPort = solarUyApiPort;
            _proveedorTiempoSensor = proveedorTiempoSensor;
        }

        public async Task<InversorDetalleDto> RecuperarInversorAsync(
            int idInversor,
            CancellationToken cancellationToken = default)
        {
            if (idInversor <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(idInversor),
                    "El identificador del inversor debe ser mayor a cero.");
            }

            var inversor = await _solarUyApiPort.ObtenerInversorAsync(
                idInversor,
                cancellationToken);

            if (inversor is null)
            {
                throw new RecursoNoEncontradoException(
                    $"No se encontro el inversor {idInversor}.");
            }

            return new InversorDetalleDto
            {
                Id = inversor.Id,
                Nombre = inversor.Nombre,
                FechaCreacion = inversor.FechaCreacion,
                FechaConsulta = _proveedorTiempoSensor.ObtenerFechaActualUtc(),
                UptimeSensor = _proveedorTiempoSensor.ObtenerUptime()
            };
        }
    }
}
