using System;
using Taller_.NET.Modulos.ParqueSolar.Domain;
using Taller_.NET.Modulos.ParqueSolar.Domain.Excepciones;
using Xunit;

namespace Taller_.NET.Tests.Modulos.ParqueSolar.Domain;

public class InversorTests
{
    [Fact]
    public void CrearInversor_ConNombreValido_AsignaPropiedades()
    {
        var antes = DateTime.UtcNow;

        var inversor = new Inversor("Inversor Central");

        var despues = DateTime.UtcNow;
        Assert.Equal("Inversor Central", inversor.Nombre);
        Assert.InRange(inversor.FechaCreacion, antes, despues);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CrearInversor_ConNombreInvalido_LanzaExcepcion(string? nombre)
    {
        Assert.Throws<InversorInvalidoException>(() => new Inversor(nombre!));
    }

    [Fact]
    public void CrearInversor_ConNombreDemasiadoLargo_LanzaExcepcion()
    {
        var nombre = new string('a', Inversor.LongitudMaximaNombre + 1);

        Assert.Throws<InversorInvalidoException>(() => new Inversor(nombre));
    }

    [Fact]
    public void CrearInversor_RecortaLosEspaciosDelNombre()
    {
        var inversor = new Inversor("  Inversor Central  ");

        Assert.Equal("Inversor Central", inversor.Nombre);
    }

    [Fact]
    public void ActualizarNombre_ConNombreValido_CambiaSoloElNombre()
    {
        var inversor = new Inversor("Inversor Central");
        var fechaCreacion = inversor.FechaCreacion;
        typeof(Inversor).GetProperty(nameof(Inversor.Id))!.SetValue(inversor, 4);

        inversor.ActualizarNombre("  Inversor Norte  ");

        Assert.Equal("Inversor Norte", inversor.Nombre);
        Assert.Equal(4, inversor.Id);
        Assert.Equal(fechaCreacion, inversor.FechaCreacion);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ActualizarNombre_ConNombreVacioOEnBlanco_LanzaExcepcion(string? nombre)
    {
        var inversor = new Inversor("Inversor Central");

        Assert.Throws<InversorInvalidoException>(() => inversor.ActualizarNombre(nombre!));
        Assert.Equal("Inversor Central", inversor.Nombre);
    }

    [Fact]
    public void ActualizarNombre_ConNombreDemasiadoLargo_LanzaExcepcion()
    {
        var inversor = new Inversor("Inversor Central");
        var nombre = new string('a', Inversor.LongitudMaximaNombre + 1);

        Assert.Throws<InversorInvalidoException>(() => inversor.ActualizarNombre(nombre));
        Assert.Equal("Inversor Central", inversor.Nombre);
    }
}
