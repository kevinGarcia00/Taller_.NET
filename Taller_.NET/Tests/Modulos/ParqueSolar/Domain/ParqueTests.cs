using System;
using Taller_.NET.Modulos.ParqueSolar.Domain;
using Taller_.NET.Modulos.ParqueSolar.Domain.Excepciones;
using Xunit;

namespace Taller_.NET.Tests.Modulos.ParqueSolar.Domain;

public class ParqueTests
{
    [Fact]
    public void CrearParque_ConDatosValidos_AsignaPropiedades()
    {
        var parque = new Parque("Parque Norte", "Salto", 10);

        Assert.Equal("Parque Norte", parque.Nombre);
        Assert.Equal("Salto", parque.Ubicacion);
        Assert.Equal(10, parque.CantMaxNodos);
    }

    [Fact]
    public void CrearParque_AsignaFechaCreacionAutomaticamente()
    {
        var antes = DateTime.UtcNow;

        var parque = new Parque("Parque Norte", "Salto", 10);

        var despues = DateTime.UtcNow;
        Assert.InRange(parque.FechaCreacion, antes, despues);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CrearParque_ConNombreInvalido_LanzaExcepcion(string nombre)
    {
        Assert.Throws<ParqueInvalidoException>(() => new Parque(nombre, "Salto", 10));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CrearParque_ConUbicacionInvalida_LanzaExcepcion(string ubicacion)
    {
        Assert.Throws<ParqueInvalidoException>(() => new Parque("Parque Norte", ubicacion, 10));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void CrearParque_ConCantMaxNodosInvalida_LanzaExcepcion(int cantMaxNodos)
    {
        Assert.Throws<ParqueInvalidoException>(() => new Parque("Parque Norte", "Salto", cantMaxNodos));
    }

    [Fact]
    public void CrearParque_ConNombreDemasiadoLargo_LanzaExcepcion()
    {
        var nombre = new string('a', Parque.LongitudMaximaNombre + 1);

        Assert.Throws<ParqueInvalidoException>(() => new Parque(nombre, "Salto", 10));
    }

    [Fact]
    public void CrearParque_ConUbicacionDemasiadoLarga_LanzaExcepcion()
    {
        var ubicacion = new string('a', Parque.LongitudMaximaUbicacion + 1);

        Assert.Throws<ParqueInvalidoException>(() => new Parque("Parque Norte", ubicacion, 10));
    }

    [Fact]
    public void CrearParque_RecortaLosEspaciosDeNombreYUbicacion()
    {
        var parque = new Parque("  Parque Norte  ", "  Salto  ", 10);

        Assert.Equal("Parque Norte", parque.Nombre);
        Assert.Equal("Salto", parque.Ubicacion);
    }
}
