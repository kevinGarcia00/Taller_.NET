using Domain.Parques;
using Domain.Parques.Excepciones;
using Xunit;

namespace Domain.Tests;

public sealed class ParqueTests
{
    [Fact]
    public void Constructor_CuandoDatosSonValidos_CreaParqueConNombreRecortado()
    {
        var fechaCreacion = new DateTime(2026, 11, 1, 12, 0, 0, DateTimeKind.Utc);

        var parque = new Parque(1, "  Parque Solar  ", "  Plaza  ", fechaCreacion, 10);

        Assert.Equal(1, parque.Id);
        Assert.Equal("Parque Solar", parque.Nombre);
        Assert.Equal("Plaza", parque.Ubicacion);
        Assert.Equal(fechaCreacion, parque.FechaCreacion);
        Assert.Equal(10, parque.CantMaxNodos);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_CuandoNombreEsVacio_LanzaParqueInvalidoException(string nombre)
    {
        Assert.Throws<ParqueInvalidoException>(() =>
            new Parque(1, nombre, "Plaza", DateTime.UtcNow, 10));
    }

    [Fact]
    public void Constructor_CuandoCantMaxNodosEsCero_LanzaParqueInvalidoException()
    {
        Assert.Throws<ParqueInvalidoException>(() =>
            new Parque(1, "Parque Solar", "Plaza", DateTime.UtcNow, 0));
    }

    [Fact]
    public void TieneMayorCapacidadQue_CuandoCapacidadEsMayor_DevuelveVerdadero()
    {
        var parque = new Parque(1, "Parque Solar", "Plaza", DateTime.UtcNow, 10);
        var otroParque = new Parque(2, "Parque Norte", "San Jose", DateTime.UtcNow, 4);

        Assert.True(parque.TieneMayorCapacidadQue(otroParque));
    }

    [Theory]
    [InlineData(10)]
    [InlineData(12)]
    public void TieneMayorCapacidadQue_CuandoCapacidadDelOtroEsMayorOIgual_DevuelveFalso(int cantMaxNodos)
    {
        var parque = new Parque(1, "Parque Solar", "Plaza", DateTime.UtcNow, 10);
        var otroParque = new Parque(2, "Parque Norte", "San Jose", DateTime.UtcNow, cantMaxNodos);

        Assert.False(parque.TieneMayorCapacidadQue(otroParque));
    }

}
