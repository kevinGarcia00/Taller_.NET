using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Modules.Sensor.Application.Dtos;
using Modules.Sensor.Application.Excepciones;
using Modules.Sensor.Application.Puertos;
using Modules.Sensor.Domain;

namespace Modules.Sensor.Application.Servicios
{
    public class ConsultaPanelService : IConsultaPanelService
    {
        private readonly ISolarUyApiPort _solarUyApiPort;

        public ConsultaPanelService(ISolarUyApiPort solarUyApiPort)
        {
            _solarUyApiPort = solarUyApiPort;
        }

        public async Task<IReadOnlyList<InversorListadoDto>> ObtenerInversoresAsync(
            CancellationToken cancellationToken = default)
        {
            var inversores = await _solarUyApiPort.ObtenerInversoresAsync(cancellationToken);

            return inversores
                .Select(inversor => new InversorListadoDto
                {
                    Id = inversor.Id,
                    Nombre = inversor.Nombre
                })
                .ToList();
        }

        public async Task<IReadOnlyList<NodoListadoDto>> ObtenerNodosInversorAsync(
            int idInversor,
            CancellationToken cancellationToken = default)
        {
            ValidarIdentificador(idInversor, nameof(idInversor));

            var nodos = await _solarUyApiPort.ObtenerNodosAsync(idInversor, cancellationToken);
            var resultado = new List<NodoListadoDto>();

            foreach (var nodo in nodos)
            {
                var paneles = await _solarUyApiPort.ObtenerPanelesAsync(
                    idInversor,
                    nodo.Id,
                    cancellationToken);

                resultado.Add(new NodoListadoDto
                {
                    Id = nodo.Id,
                    Nombre = nodo.Nombre,
                    IdInversor = nodo.IdInversor,
                    Paneles = paneles.Select(CrearPanelListado).ToList()
                });
            }

            return resultado;
        }

        public async Task<IReadOnlyList<PanelListadoDto>> ObtenerPanelesNodoInversorAsync(
            int idInversor,
            int idNodo,
            CancellationToken cancellationToken = default)
        {
            ValidarIdentificador(idInversor, nameof(idInversor));
            ValidarIdentificador(idNodo, nameof(idNodo));

            var paneles = await _solarUyApiPort.ObtenerPanelesAsync(
                idInversor,
                idNodo,
                cancellationToken);

            return paneles
                .Select(CrearPanelListado)
                .ToList();
        }

        public async Task<PanelDetalleDto> ObtenerPanelAsync(
            int idInversor,
            int idNodo,
            int idPanel,
            CancellationToken cancellationToken = default)
        {
            ValidarIdentificador(idInversor, nameof(idInversor));
            ValidarIdentificador(idNodo, nameof(idNodo));
            ValidarIdentificador(idPanel, nameof(idPanel));

            var panel = await _solarUyApiPort.ObtenerPanelAsync(
                idInversor,
                idNodo,
                idPanel,
                cancellationToken);

            if (panel is null)
            {
                throw new RecursoNoEncontradoException(
                    $"No se encontro el panel {idPanel} en el nodo {idNodo} del inversor {idInversor}.");
            }

            return new PanelDetalleDto
            {
                Id = panel.Id,
                Nombre = panel.Nombre,
                FechaCreacion = panel.FechaCreacion,
                EstadoPanel = panel.Estado.ToString(),
                Encendido = panel.Encendido,
                VolcarEnergia = panel.VolcarEnergia,
                Potencia = panel.Potencia,
                VidaUtil = panel.VidaUtil,
                IdInversor = idInversor,
                IdNodo = idNodo
            };
        }

        private static PanelListadoDto CrearPanelListado(Panel panel)
        {
            return new PanelListadoDto
            {
                Id = panel.Id,
                Nombre = panel.Nombre,
                EstadoPanel = panel.Estado.ToString(),
                Encendido = panel.Encendido,
                VolcarEnergia = panel.VolcarEnergia,
                Potencia = panel.Potencia
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
