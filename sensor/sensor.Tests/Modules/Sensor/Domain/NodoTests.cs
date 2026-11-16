using Modules.Sensor.Domain;
using Modules.Sensor.Domain.Excepciones;
using Xunit;

namespace sensor.Tests.Modules.Sensor.Domain
{
    public class NodoTests
    {
        [Fact]
        public void CrearNodo_ConDatosValidos_AsignaPropiedades()
        {
            var nodo = new Nodo(4, "  Nodo Norte  ", 2);

            Assert.Equal(4, nodo.Id);
            Assert.Equal("Nodo Norte", nodo.Nombre);
            Assert.Equal(2, nodo.IdInversor);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void CrearNodo_ConIdInvalido_LanzaExcepcion(int id)
        {
            Assert.Throws<NodoInvalidoException>(() => new Nodo(id, "Nodo Norte", 1));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void CrearNodo_ConNombreInvalido_LanzaExcepcion(string? nombre)
        {
            Assert.Throws<NodoInvalidoException>(() => new Nodo(1, nombre!, 1));
        }

        [Fact]
        public void CrearNodo_ConNombreDemasiadoLargo_LanzaExcepcion()
        {
            var nombre = new string('a', Nodo.LongitudMaximaNombre + 1);

            Assert.Throws<NodoInvalidoException>(() => new Nodo(1, nombre, 1));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void CrearNodo_ConInversorInvalido_LanzaExcepcion(int idInversor)
        {
            Assert.Throws<NodoInvalidoException>(() => new Nodo(1, "Nodo Norte", idInversor));
        }
    }
}
