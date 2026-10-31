using System;
using System.Reflection;
using Taller_.NET.Modulos.ParqueSolar.Domain;
using Taller_.NET.Modulos.ParqueSolar.Domain.Excepciones;
using Xunit;

namespace Taller_.NET.Tests.Modulos.ParqueSolar.Domain;

public class PanelTests
{
    [Fact]
    public void CrearPanel_ConDatosValidos_AsignaPropiedadesYFechaDeCreacion()
    {
        var antes = DateTime.UtcNow;

        var panel = new Panel("Panel Norte", EstadoPanel.Encendido, 450.5m, "25 anos");

        var despues = DateTime.UtcNow;
        Assert.Equal("Panel Norte", panel.Nombre);
        Assert.Equal(EstadoPanel.Encendido, panel.Estado);
        Assert.Equal(450.5m, panel.Potencia);
        Assert.Equal("25 anos", panel.VidaUtil);
        Assert.InRange(panel.FechaCreacion, antes, despues);
    }

    [Fact]
    public void CrearPanel_RecortaLosEspaciosDelNombreYDeLaVidaUtil()
    {
        var panel = new Panel("  Panel Norte  ", EstadoPanel.Sucio, 1m, "  25 anos  ");

        Assert.Equal("Panel Norte", panel.Nombre);
        Assert.Equal("25 anos", panel.VidaUtil);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CrearPanel_ConNombreInvalido_LanzaExcepcion(string? nombre)
    {
        Assert.Throws<PanelInvalidoException>(() => new Panel(nombre!, EstadoPanel.Sucio, 1m, "25 anos"));
    }

    [Fact]
    public void CrearPanel_ConNombreDemasiadoLargo_LanzaExcepcion()
    {
        var nombre = new string('a', Panel.LongitudMaximaNombre + 1);

        Assert.Throws<PanelInvalidoException>(() => new Panel(nombre, EstadoPanel.Sucio, 1m, "25 anos"));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void CrearPanel_ConPotenciaNoPositiva_LanzaExcepcion(int potencia)
    {
        Assert.Throws<PanelInvalidoException>(() => new Panel("Panel Norte", EstadoPanel.Roto, potencia, "25 anos"));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CrearPanel_ConVidaUtilInvalida_LanzaExcepcion(string? vidaUtil)
    {
        Assert.Throws<PanelInvalidoException>(() => new Panel("Panel Norte", EstadoPanel.Apagado, 1m, vidaUtil!));
    }

    [Fact]
    public void CrearPanel_ConVidaUtilDemasiadoLarga_LanzaExcepcion()
    {
        var vidaUtil = new string('a', Panel.LongitudMaximaVidaUtil + 1);

        Assert.Throws<PanelInvalidoException>(() => new Panel("Panel Norte", EstadoPanel.Deteriorado, 1m, vidaUtil));
    }

    [Fact]
    public void CrearPanel_ConEstadoIndefinido_LanzaExcepcion()
    {
        Assert.Throws<PanelInvalidoException>(() => new Panel("Panel Norte", (EstadoPanel)99, 1m, "25 anos"));
    }

    [Fact]
    public void AsignarNodo_ConNodoRegistrado_VinculaElPanelAlNodo()
    {
        var panel = new Panel("Panel Norte", EstadoPanel.Encendido, 450m, "25 anos");
        var nodo = CrearNodoRegistrado(8);

        panel.AsignarNodo(nodo);

        Assert.Same(nodo, panel.Nodo);
        Assert.Equal(8, panel.NodoId);
    }

    [Fact]
    public void AsignarNodo_ConNodoNulo_LanzaExcepcion()
    {
        var panel = new Panel("Panel Norte", EstadoPanel.Encendido, 450m, "25 anos");

        Assert.Throws<PanelInvalidoException>(() => panel.AsignarNodo(null!));
    }

    [Fact]
    public void AsignarNodo_ConNodoNoRegistrado_LanzaExcepcion()
    {
        var panel = new Panel("Panel Norte", EstadoPanel.Encendido, 450m, "25 anos");
        var nodo = new Nodo("Nodo Norte", "Sector A");

        Assert.Throws<PanelInvalidoException>(() => panel.AsignarNodo(nodo));
    }

    [Fact]
    public void ActualizarDatos_ConDatosValidos_CambiaLosCamposEditablesYConservaLaIdentidad()
    {
        var panel = new Panel("Panel Norte", EstadoPanel.Encendido, 450m, "25 anos");
        var nodo = CrearNodoRegistrado(8);
        panel.AsignarNodo(nodo);
        var fechaCreacion = panel.FechaCreacion;

        panel.ActualizarDatos("Panel Sur", EstadoPanel.Sucio, 510.25m, "20 anos");

        Assert.Equal("Panel Sur", panel.Nombre);
        Assert.Equal(EstadoPanel.Sucio, panel.Estado);
        Assert.Equal(510.25m, panel.Potencia);
        Assert.Equal("20 anos", panel.VidaUtil);
        Assert.Equal(fechaCreacion, panel.FechaCreacion);
        Assert.Same(nodo, panel.Nodo);
        Assert.Equal(8, panel.NodoId);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ActualizarDatos_ConNombreVacio_LanzaExcepcion(string? nombre)
    {
        var panel = new Panel("Panel Norte", EstadoPanel.Encendido, 450m, "25 anos");

        Assert.Throws<PanelInvalidoException>(() => panel.ActualizarDatos(nombre!, EstadoPanel.Roto, 10m, "20 anos"));
        Assert.Equal("Panel Norte", panel.Nombre);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void ActualizarDatos_ConPotenciaNoPositiva_LanzaExcepcion(int potencia)
    {
        var panel = new Panel("Panel Norte", EstadoPanel.Encendido, 450m, "25 anos");

        Assert.Throws<PanelInvalidoException>(() => panel.ActualizarDatos("Panel Norte", EstadoPanel.Apagado, potencia, "20 anos"));
        Assert.Equal(450m, panel.Potencia);
    }

    private static Nodo CrearNodoRegistrado(int id)
    {
        var nodo = new Nodo("Nodo Norte", "Sector A");
        typeof(Nodo).GetProperty(nameof(Nodo.Id), BindingFlags.Instance | BindingFlags.Public)!
            .SetValue(nodo, id);

        return nodo;
    }
}
