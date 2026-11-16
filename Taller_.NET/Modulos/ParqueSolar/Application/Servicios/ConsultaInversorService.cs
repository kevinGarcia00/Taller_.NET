using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Taller_.NET.Modulos.ParqueSolar.Application.Dtos;
using Taller_.NET.Modulos.ParqueSolar.Application.Excepciones;
using Taller_.NET.Modulos.ParqueSolar.Application.Puertos;
using Taller_.NET.Modulos.ParqueSolar.Domain;
using InversorDomain = Taller_.NET.Modulos.ParqueSolar.Domain.Inversor;
using NodoDomain = Taller_.NET.Modulos.ParqueSolar.Domain.Nodo;
using PanelDomain = Taller_.NET.Modulos.ParqueSolar.Domain.Panel;

namespace Taller_.NET.Modulos.ParqueSolar.Application.Servicios
{
    public class ConsultaInversorService : IConsultaInversorService
    {
        private readonly IConsultaInversorRepository _consultaInversorRepository;

        public ConsultaInversorService(IConsultaInversorRepository consultaInversorRepository)
        {
            _consultaInversorRepository = consultaInversorRepository;
        }

        public async Task<IReadOnlyList<InversorConsultaDto>> ObtenerInversoresAsync(
            CancellationToken cancellationToken = default)
        {
            var inversores = await _consultaInversorRepository.ObtenerInversoresAsync(cancellationToken);

            return inversores
                .Select(CrearInversorConsulta)
                .ToList();
        }

        public async Task<InversorConsultaDto?> ObtenerInversorAsync(
            int idInversor,
            CancellationToken cancellationToken = default)
        {
            ValidarIdentificador(idInversor, nameof(idInversor));

            var inversor = await _consultaInversorRepository.ObtenerInversorPorIdAsync(
                idInversor,
                cancellationToken);

            return inversor is null ? null : CrearInversorConsulta(inversor);
        }

        public async Task<IReadOnlyList<NodoConsultaDto>> ObtenerNodosInversorAsync(
            int idInversor,
            CancellationToken cancellationToken = default)
        {
            ValidarIdentificador(idInversor, nameof(idInversor));
            await ObtenerInversorRegistradoAsync(idInversor, cancellationToken);

            var nodos = await _consultaInversorRepository.ObtenerNodosPorInversorAsync(
                idInversor,
                cancellationToken);

            return nodos
                .Select(CrearNodoConsulta)
                .ToList();
        }

        public async Task<IReadOnlyList<PanelConsultaDto>> ObtenerPanelesNodoInversorAsync(
            int idInversor,
            int idNodo,
            CancellationToken cancellationToken = default)
        {
            var nodo = await ObtenerNodoDelInversorAsync(idInversor, idNodo, cancellationToken);

            var paneles = await _consultaInversorRepository.ObtenerPanelesPorNodoAsync(
                idNodo,
                cancellationToken);

            return paneles
                .Select(panel => CrearPanelConsulta(panel, idInversor, nodo.ParqueSolarId))
                .ToList();
        }

        public async Task<PanelConsultaDto?> ObtenerPanelInversorAsync(
            int idInversor,
            int idNodo,
            int idPanel,
            CancellationToken cancellationToken = default)
        {
            ValidarIdentificador(idPanel, nameof(idPanel));
            var nodo = await ObtenerNodoDelInversorAsync(idInversor, idNodo, cancellationToken);

            var panel = await _consultaInversorRepository.ObtenerPanelPorIdYNodoAsync(
                idPanel,
                idNodo,
                cancellationToken);

            return panel is null ? null : CrearPanelConsulta(panel, idInversor, nodo.ParqueSolarId);
        }

        private async Task<NodoDomain> ObtenerNodoDelInversorAsync(
            int idInversor,
            int idNodo,
            CancellationToken cancellationToken)
        {
            ValidarIdentificador(idInversor, nameof(idInversor));
            ValidarIdentificador(idNodo, nameof(idNodo));
            await ObtenerInversorRegistradoAsync(idInversor, cancellationToken);

            var nodo = await _consultaInversorRepository.ObtenerNodoPorIdAsync(idNodo, cancellationToken);

            if (nodo is null || nodo.InversorId != idInversor)
            {
                throw new NodoNoEncontradoException(idNodo);
            }

            return nodo;
        }

        private async Task<InversorDomain> ObtenerInversorRegistradoAsync(
            int idInversor,
            CancellationToken cancellationToken)
        {
            var inversor = await _consultaInversorRepository.ObtenerInversorPorIdAsync(
                idInversor,
                cancellationToken);

            if (inversor is null)
            {
                throw new InversorNoEncontradoException(idInversor);
            }

            return inversor;
        }

        private static InversorConsultaDto CrearInversorConsulta(InversorDomain inversor)
        {
            return new InversorConsultaDto
            {
                Id = inversor.Id,
                Nombre = inversor.Nombre,
                FechaCreacion = inversor.FechaCreacion
            };
        }

        private static NodoConsultaDto CrearNodoConsulta(NodoDomain nodo)
        {
            return new NodoConsultaDto
            {
                Id = nodo.Id,
                Nombre = nodo.Nombre,
                FechaCreacion = nodo.FechaCreacion,
                IdInversor = nodo.InversorId ?? 0,
                IdParqueSolar = nodo.ParqueSolarId
            };
        }

        private static PanelConsultaDto CrearPanelConsulta(
            PanelDomain panel,
            int idInversor,
            int idParqueSolar)
        {
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
                IdParqueSolar = idParqueSolar
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
