using System;
using Modules.Sensor.Domain;
using Modules.Sensor.Domain.Excepciones;
using Xunit;

namespace sensor.Tests.Modules.Sensor.Domain
{
    public class InversorTests
    {
        [Fact]
        public void CrearInversor_ConDatosValidos_AsignaPropiedades()
        {
            var fechaCreacion = DateTime.UtcNow.AddDays(-3);

            var inversor = new Inversor(3, "  Inversor Central  ", fechaCreacion);

            Assert.Equal(3, inversor.Id);
            Assert.Equal("Inversor Central", inversor.Nombre);
            Assert.Equal(fechaCreacion, inversor.FechaCreacion);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void CrearInversor_ConIdInvalido_LanzaExcepcion(int id)
        {
            Assert.Throws<InversorInvalidoException>(
                () => new Inversor(id, "Inversor Central", DateTime.UtcNow.AddDays(-1)));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void CrearInversor_ConNombreInvalido_LanzaExcepcion(string? nombre)
        {
            Assert.Throws<InversorInvalidoException>(
                () => new Inversor(1, nombre!, DateTime.UtcNow.AddDays(-1)));
        }

        [Fact]
        public void CrearInversor_ConNombreDemasiadoLargo_LanzaExcepcion()
        {
            var nombre = new string('a', Inversor.LongitudMaximaNombre + 1);

            Assert.Throws<InversorInvalidoException>(
                () => new Inversor(1, nombre, DateTime.UtcNow.AddDays(-1)));
        }

        [Fact]
        public void CrearInversor_ConFechaPorDefecto_LanzaExcepcion()
        {
            Assert.Throws<InversorInvalidoException>(
                () => new Inversor(1, "Inversor Central", default));
        }

        [Fact]
        public void CrearInversor_ConFechaFutura_LanzaExcepcion()
        {
            Assert.Throws<InversorInvalidoException>(
                () => new Inversor(1, "Inversor Central", DateTime.UtcNow.AddDays(1)));
        }
    }
}
