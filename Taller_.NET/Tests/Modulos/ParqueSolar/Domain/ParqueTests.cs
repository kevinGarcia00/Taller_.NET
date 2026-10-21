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

    [Fact]
    public void ObtenerCantidadNodosActuales_ConNodosRegistrados_RetornaLaCantidad()
    {
        var parque = new Parque("Parque Norte", "Salto", 10);

        var cantidad = parque.ObtenerCantidadNodosActuales(4);

        Assert.Equal(4, cantidad);
    }

    [Fact]
    public void ObtenerCantidadNodosActuales_ConCero_RetornaCero()
    {
        var parque = new Parque("Parque Norte", "Salto", 10);

        var cantidad = parque.ObtenerCantidadNodosActuales(0);

        Assert.Equal(0, cantidad);
    }

    [Fact]
    public void ObtenerCantidadNodosActuales_ConValorNegativo_LanzaExcepcion()
    {
        var parque = new Parque("Parque Norte", "Salto", 10);

        Assert.Throws<ParqueInvalidoException>(() => parque.ObtenerCantidadNodosActuales(-1));
    }

    [Fact]
    public void Modificar_ConDatosValidos_ActualizaPropiedades()
    {
        var parque = new Parque("Parque Norte", "Salto", 10);

        parque.Modificar("Parque Sur", "Montevideo", 25);

        Assert.Equal("Parque Sur", parque.Nombre);
        Assert.Equal("Montevideo", parque.Ubicacion);
        Assert.Equal(25, parque.CantMaxNodos);
    }

    [Fact]
    public void Modificar_NoModificaIdNiFechaCreacion()
    {
        var parque = new Parque("Parque Norte", "Salto", 10);
        var fechaCreacion = parque.FechaCreacion;

        parque.Modificar("Parque Sur", "Montevideo", 25);

        Assert.Equal(0, parque.Id);
        Assert.Equal(fechaCreacion, parque.FechaCreacion);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Modificar_ConNombreInvalido_LanzaExcepcion(string? nombre)
    {
        var parque = new Parque("Parque Norte", "Salto", 10);

        Assert.Throws<ParqueInvalidoException>(() => parque.Modificar(nombre!, "Montevideo", 25));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Modificar_ConUbicacionInvalida_LanzaExcepcion(string? ubicacion)
    {
        var parque = new Parque("Parque Norte", "Salto", 10);

        Assert.Throws<ParqueInvalidoException>(() => parque.Modificar("Parque Sur", ubicacion!, 25));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Modificar_ConCantMaxNodosInvalida_LanzaExcepcion(int cantMaxNodos)
    {
        var parque = new Parque("Parque Norte", "Salto", 10);

        Assert.Throws<ParqueInvalidoException>(() => parque.Modificar("Parque Sur", "Montevideo", cantMaxNodos));
    }

    [Fact]
    public void Modificar_ConNombreDemasiadoLargo_LanzaExcepcion()
    {
        var parque = new Parque("Parque Norte", "Salto", 10);
        var nombre = new string('a', Parque.LongitudMaximaNombre + 1);

        Assert.Throws<ParqueInvalidoException>(() => parque.Modificar(nombre, "Montevideo", 25));
    }

    [Fact]
    public void Modificar_ConUbicacionDemasiadoLarga_LanzaExcepcion()
    {
        var parque = new Parque("Parque Norte", "Salto", 10);
        var ubicacion = new string('a', Parque.LongitudMaximaUbicacion + 1);

        Assert.Throws<ParqueInvalidoException>(() => parque.Modificar("Parque Sur", ubicacion, 25));
    }

    [Fact]
    public void Modificar_ConDatosInvalidos_NoCambiaLasPropiedades()
    {
        var parque = new Parque("Parque Norte", "Salto", 10);

        Assert.Throws<ParqueInvalidoException>(() => parque.Modificar("", "Montevideo", 25));

        Assert.Equal("Parque Norte", parque.Nombre);
        Assert.Equal("Salto", parque.Ubicacion);
        Assert.Equal(10, parque.CantMaxNodos);
    }

    [Fact]
    public void Modificar_RecortaLosEspaciosDeNombreYUbicacion()
    {
        var parque = new Parque("Parque Norte", "Salto", 10);

        parque.Modificar("  Parque Sur  ", "  Montevideo  ", 25);

        Assert.Equal("Parque Sur", parque.Nombre);
        Assert.Equal("Montevideo", parque.Ubicacion);
    }

    [Fact]
    public void ValidarCapacidadNodos_ConNodosDentroDeLaCapacidad_NoLanzaExcepcion()
    {
        var parque = new Parque("Parque Norte", "Salto", 10);

        parque.ValidarCapacidadNodos(10);
    }

    [Fact]
    public void ValidarCapacidadNodos_ConNodosPorEncimaDeLaCapacidad_LanzaExcepcion()
    {
        var parque = new Parque("Parque Norte", "Salto", 10);

        Assert.Throws<ParqueInvalidoException>(() => parque.ValidarCapacidadNodos(11));
    }

    [Fact]
    public void ValidarCapacidadNodos_ConCeroNodos_NoLanzaExcepcion()
    {
        var parque = new Parque("Parque Norte", "Salto", 10);

        parque.ValidarCapacidadNodos(0);
    }

    [Fact]
    public void ValidarCapacidadNodos_ConNodosNegativos_LanzaExcepcion()
    {
        var parque = new Parque("Parque Norte", "Salto", 10);

        Assert.Throws<ParqueInvalidoException>(() => parque.ValidarCapacidadNodos(-1));
    }

    [Fact]
    public void ValidarCapacidadNodos_ConCapacidadPropuestaQueAlojaLosNodosRegistrados_NoLanzaExcepcion()
    {
        var parque = new Parque("Parque Norte", "Salto", 10);

        parque.ValidarCapacidadNodos(3, 5);
    }

    [Fact]
    public void ValidarCapacidadNodos_ConCapacidadPropuestaInferiorALosNodosRegistrados_LanzaExcepcion()
    {
        var parque = new Parque("Parque Norte", "Salto", 10);

        Assert.Throws<ParqueInvalidoException>(() => parque.ValidarCapacidadNodos(3, 2));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void ValidarCapacidadNodos_ConCapacidadPropuestaInvalida_LanzaExcepcion(int cantMaxNodos)
    {
        var parque = new Parque("Parque Norte", "Salto", 10);

        Assert.Throws<ParqueInvalidoException>(() => parque.ValidarCapacidadNodos(3, cantMaxNodos));
    }

    [Fact]
    public void ValidarCapacidadNodos_ConNodosRegistradosNegativosYCapacidadPropuesta_LanzaExcepcion()
    {
        var parque = new Parque("Parque Norte", "Salto", 10);

        Assert.Throws<ParqueInvalidoException>(() => parque.ValidarCapacidadNodos(-1, 5));
    }
}
