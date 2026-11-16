using System;
using Taller_.NET.Modulos.Historico.Domain;
using Taller_.NET.Modulos.Historico.Domain.Excepciones;
using Xunit;

namespace Taller_.NET.Tests.Modulos.Historico.Domain;

public class HistoricoMantenibilidadTests
{
    private static readonly Guid MantenedorId = Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");

    [Fact]
    public void CrearRegistro_ConDatosValidos_AsignaPropiedadesSinModificarElContexto()
    {
        var fecha = DateTime.Today;
        var mantenedor = new Mantenedor(MantenedorId);
        var contexto = new ContextoPanel(4, 2, 1);

        var registro = new HistoricoMantenibilidad(
            mantenedor,
            fecha.AddHours(15),
            "  Limpieza de superficie  ",
            EstadoPanelRegistrado.Sucio,
            EstadoPanelRegistrado.Encendido,
            contexto);

        Assert.Equal(MantenedorId, registro.Mantenedor.IdMantenedor);
        Assert.Equal(fecha, registro.Fecha);
        Assert.Equal("Limpieza de superficie", registro.Observaciones);
        Assert.Equal(EstadoPanelRegistrado.Sucio, registro.EstadoInicial);
        Assert.Equal(EstadoPanelRegistrado.Encendido, registro.EstadoResultante);
        Assert.Equal(4, registro.ContextoPanel.PanelId);
        Assert.Equal(2, registro.ContextoPanel.NodoId);
        Assert.Equal(1, registro.ContextoPanel.ParqueSolarId);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CrearRegistro_ConObservacionesVacias_LanzaExcepcion(string? observaciones)
    {
        Assert.Throws<HistoricoMantenibilidadInvalidoException>(() => Crear(observaciones: observaciones!));
    }

    [Fact]
    public void CrearRegistro_ConObservacionesDemasiadoLargas_LanzaExcepcion()
    {
        var observaciones = new string('a', HistoricoMantenibilidad.LongitudMaximaObservaciones + 1);

        Assert.Throws<HistoricoMantenibilidadInvalidoException>(() => Crear(observaciones: observaciones));
    }

    [Fact]
    public void CrearRegistro_ConFechaPosteriorAHoy_LanzaExcepcion()
    {
        Assert.Throws<HistoricoMantenibilidadInvalidoException>(() =>
            Crear(fecha: DateTime.Today.AddDays(1)));
    }

    [Fact]
    public void CrearRegistro_ConFechaDeHoy_EsValido()
    {
        var registro = Crear(fecha: DateTime.Today);

        Assert.Equal(DateTime.Today, registro.Fecha);
    }

    [Fact]
    public void CrearRegistro_ConEstadoIndefinido_LanzaExcepcion()
    {
        Assert.Throws<HistoricoMantenibilidadInvalidoException>(() =>
            Crear(estadoResultante: (EstadoPanelRegistrado)99));
    }

    [Fact]
    public void CrearMantenedor_ConIdentificadorVacio_LanzaExcepcion()
    {
        Assert.Throws<HistoricoMantenibilidadInvalidoException>(() => new Mantenedor(Guid.Empty));
    }

    [Theory]
    [InlineData(0, 1, 1)]
    [InlineData(1, 0, 1)]
    [InlineData(1, 1, 0)]
    public void CrearContexto_ConIdentificadorInvalido_LanzaExcepcion(int panelId, int nodoId, int parqueSolarId)
    {
        Assert.Throws<HistoricoInvalidoException>(() =>
            new ContextoPanel(panelId, nodoId, parqueSolarId));
    }

    private static HistoricoMantenibilidad Crear(
        string observaciones = "Se limpio el panel.",
        DateTime? fecha = null,
        EstadoPanelRegistrado estadoResultante = EstadoPanelRegistrado.Encendido)
    {
        return new HistoricoMantenibilidad(
            new Mantenedor(MantenedorId),
            fecha ?? DateTime.Today,
            observaciones,
            EstadoPanelRegistrado.Sucio,
            estadoResultante,
            new ContextoPanel(4, 2, 1));
    }
}
