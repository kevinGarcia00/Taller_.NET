using System;
using System.Threading;
using System.Threading.Tasks;
using Taller_.NET.Modulos.ParqueSolar.Application.Dtos;
using Taller_.NET.Modulos.ParqueSolar.Application.Excepciones;
using Taller_.NET.Modulos.ParqueSolar.Application.Puertos;
using Taller_.NET.Modulos.ParqueSolar.Domain;

namespace Taller_.NET.Modulos.ParqueSolar.Application.Servicios
{
    public class CambiarVolcadoEnergiaPanelService : ICambiarVolcadoEnergiaPanelService
    {
        private readonly IConsultaInversorRepository _consultaInversorRepository;
        private readonly IPanelRepository _panelRepository;

        public CambiarVolcadoEnergiaPanelService(
            IConsultaInversorRepository consultaInversorRepository,
            IPanelRepository panelRepository)
        {
            _consultaInversorRepository = consultaInversorRepository;
            _panelRepository = panelRepository;
        }

        public async Task<PanelConsultaDto> CambiarVolcadoEnergiaPanelAsync(
            int idInversor,
            int idNodo,
            int idPanel,
            CambiarVolcadoEnergiaPanelDto dto,
            CancellationToken cancellationToken = default)
        {
            ValidarIdentificador(idInversor, nameof(idInversor));
            ValidarIdentificador(idNodo, nameof(idNodo));
            ValidarIdentificador(idPanel, nameof(idPanel));

            if (dto.VolcarEnergia is null)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(dto.VolcarEnergia),
                    "El valor de volcarEnergia es obligatorio.");
            }

            var inversor = await _consultaInversorRepository.ObtenerInversorPorIdAsync(
                idInversor,
                cancellationToken);

            if (inversor is null)
            {
                throw new InversorNoEncontradoException(idInversor);
            }

            var nodo = await _consultaInversorRepository.ObtenerNodoPorIdAsync(
                idNodo,
                cancellationToken);

            if (nodo is null || nodo.InversorId != idInversor)
            {
                throw new NodoNoEncontradoException(idNodo);
            }

            var panel = await _consultaInversorRepository.ObtenerPanelPorIdYNodoAsync(
                idPanel,
                idNodo,
                cancellationToken);

            if (panel is null)
            {
                throw new PanelNoEncontradoException(idPanel);
            }

            panel.CambiarVolcadoEnergia(dto.VolcarEnergia.Value);
            await _panelRepository.ActualizarAsync(panel, cancellationToken);

            return new PanelConsultaDto
            {
                Id = panel.Id,
                Nombre = panel.Nombre,
                FechaCreacion = panel.FechaCreacion,
                EstadoPanel = panel.Estado.ToString(),
                Encendido = panel.Estado == EstadoPanel.Encendido,
                VolcarEnergia = panel.VolcarEnergia,
                Potencia = panel.Potencia,
                VidaUtil = panel.VidaUtil,
                IdInversor = idInversor,
                IdNodo = panel.NodoId,
                IdParqueSolar = nodo.ParqueSolarId
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
