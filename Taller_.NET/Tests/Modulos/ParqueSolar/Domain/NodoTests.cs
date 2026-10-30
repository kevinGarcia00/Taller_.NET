using System;
using Taller_.NET.Modulos.ParqueSolar.Domain;
using Taller_.NET.Modulos.ParqueSolar.Domain.Excepciones;
using Xunit;

namespace Taller_.NET.Tests.Modulos.ParqueSolar.Domain;

public class NodoTests
{
    [Fact]
    public void CrearNodo_ConDatosValidos_AsignaPropiedades()
    {
        var nodo = new Nodo("Nodo Norte", "Sector A");

        Assert.Equal("Nodo Norte", nodo.Nombre);
        Assert.Equal("Sector A", nodo.UbicacionParque);
        Assert.Null(nodo.InversorId);
    }

    [Fact]
    public void CrearNodo_AsignaFechaCreacionEInicializaEnergiaEnCero()
    {
        var antes = DateTime.UtcNow;

        var nodo = new Nodo("Nodo Norte", "Sector A");

        var despues = DateTime.UtcNow;
        Assert.InRange(nodo.FechaCreacion, antes, despues);
        Assert.Equal(0m, nodo.EnergiaGenerada);
    }

    [Fact]
    public void CrearNodo_ConInversor_AsignaInversorId()
    {
        var nodo = new Nodo("Nodo Norte", "Sector A", 7);

        Assert.Equal(7, nodo.InversorId);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CrearNodo_ConNombreInvalido_LanzaExcepcion(string? nombre)
    {
        Assert.Throws<NodoInvalidoException>(() => new Nodo(nombre!, "Sector A"));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CrearNodo_ConUbicacionInvalida_LanzaExcepcion(string? ubicacionParque)
    {
        Assert.Throws<NodoInvalidoException>(() => new Nodo("Nodo Norte", ubicacionParque!));
    }

    [Fact]
    public void CrearNodo_ConNombreDemasiadoLargo_LanzaExcepcion()
    {
        var nombre = new string('a', Nodo.LongitudMaximaNombre + 1);

        Assert.Throws<NodoInvalidoException>(() => new Nodo(nombre, "Sector A"));
    }

    [Fact]
    public void CrearNodo_ConUbicacionDemasiadoLarga_LanzaExcepcion()
    {
        var ubicacionParque = new string('a', Nodo.LongitudMaximaUbicacionParque + 1);

        Assert.Throws<NodoInvalidoException>(() => new Nodo("Nodo Norte", ubicacionParque));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void CrearNodo_ConInversorIdInvalido_LanzaExcepcion(int inversorId)
    {
        Assert.Throws<NodoInvalidoException>(() => new Nodo("Nodo Norte", "Sector A", inversorId));
    }

    [Fact]
    public void CrearNodo_RecortaLosEspaciosDeNombreYUbicacion()
    {
        var nodo = new Nodo("  Nodo Norte  ", "  Sector A  ");

        Assert.Equal("Nodo Norte", nodo.Nombre);
        Assert.Equal("Sector A", nodo.UbicacionParque);
    }

    [Fact]
    public void AsignarContexto_ConContextoValido_AsignaElParqueSolar()
    {
        var nodo = new Nodo("Nodo Norte", "Sector A");

        nodo.AsignarContexto(new ContextoParque(5));

        Assert.Equal(5, nodo.ContextoParque.ParqueSolarId);
    }

    [Fact]
    public void AsignarContexto_ConContextoNulo_LanzaExcepcion()
    {
        var nodo = new Nodo("Nodo Norte", "Sector A");

        Assert.Throws<NodoInvalidoException>(() => nodo.AsignarContexto(null!));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void CrearContextoParque_ConParqueSolarIdInvalido_LanzaExcepcion(int parqueSolarId)
    {
        Assert.Throws<NodoInvalidoException>(() => new ContextoParque(parqueSolarId));
    }

    [Fact]
    public void Actualizar_ConDatosValidos_ActualizaNombreUbicacionEInversor()
    {
        var nodo = new Nodo("Nodo Norte", "Sector A", 1);
        nodo.AsignarContexto(new ContextoParque(5));
        var fechaCreacion = nodo.FechaCreacion;

        nodo.Actualizar("Nodo Sur", "Sector B", 9);

        Assert.Equal("Nodo Sur", nodo.Nombre);
        Assert.Equal("Sector B", nodo.UbicacionParque);
        Assert.Equal(9, nodo.InversorId);
        Assert.Equal(5, nodo.ContextoParque!.ParqueSolarId);
        Assert.Equal(fechaCreacion, nodo.FechaCreacion);
        Assert.Equal(0m, nodo.EnergiaGenerada);
    }

    [Fact]
    public void Actualizar_ConInversorNulo_QuitaElInversorAsignado()
    {
        var nodo = new Nodo("Nodo Norte", "Sector A", 1);

        nodo.Actualizar("Nodo Sur", "Sector B", null);

        Assert.Null(nodo.InversorId);
    }

    [Fact]
    public void Actualizar_RecortaLosEspaciosDeNombreYUbicacion()
    {
        var nodo = new Nodo("Nodo Norte", "Sector A", 1);

        nodo.Actualizar("  Nodo Sur  ", "  Sector B  ", 2);

        Assert.Equal("Nodo Sur", nodo.Nombre);
        Assert.Equal("Sector B", nodo.UbicacionParque);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Actualizar_ConNombreInvalido_LanzaExcepcion(string? nombre)
    {
        var nodo = new Nodo("Nodo Norte", "Sector A", 1);

        Assert.Throws<NodoInvalidoException>(() => nodo.Actualizar(nombre!, "Sector B", 2));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Actualizar_ConUbicacionInvalida_LanzaExcepcion(string? ubicacionParque)
    {
        var nodo = new Nodo("Nodo Norte", "Sector A", 1);

        Assert.Throws<NodoInvalidoException>(() => nodo.Actualizar("Nodo Sur", ubicacionParque!, 2));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Actualizar_ConInversorIdInvalido_LanzaExcepcion(int inversorId)
    {
        var nodo = new Nodo("Nodo Norte", "Sector A", 1);

        Assert.Throws<NodoInvalidoException>(() => nodo.Actualizar("Nodo Sur", "Sector B", inversorId));
    }
}
