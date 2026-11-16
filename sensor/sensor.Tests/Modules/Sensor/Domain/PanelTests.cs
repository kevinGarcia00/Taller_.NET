using System;
using Modules.Sensor.Domain;
using Modules.Sensor.Domain.Excepciones;
using Xunit;

namespace sensor.Tests.Modules.Sensor.Domain
{
    public class PanelTests
    {
        [Fact]
        public void CrearPanel_ConDatosValidos_AsignaPropiedades()
        {
            var fechaCreacion = DateTime.UtcNow.AddDays(-2);

            var panel = new Panel(
                10,
                "  Panel Norte  ",
                fechaCreacion,
                EstadoPanel.Encendido,
                true,
                450.5m,
                "  25 anos  ",
                8);

            Assert.Equal(10, panel.Id);
            Assert.Equal("Panel Norte", panel.Nombre);
            Assert.Equal(fechaCreacion, panel.FechaCreacion);
            Assert.Equal(EstadoPanel.Encendido, panel.Estado);
            Assert.True(panel.Encendido);
            Assert.False(panel.VolcarEnergia);
            Assert.Equal(450.5m, panel.Potencia);
            Assert.Equal("25 anos", panel.VidaUtil);
            Assert.Equal(8, panel.IdNodo);
        }

        [Fact]
        public void CrearPanel_ConVolcadoHabilitado_AsignaElValor()
        {
            var panel = CrearPanel(volcarEnergia: true);

            Assert.True(panel.VolcarEnergia);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void CrearPanel_ConIdInvalido_LanzaExcepcion(int id)
        {
            Assert.Throws<PanelInvalidoException>(() => CrearPanel(id: id));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void CrearPanel_ConNombreInvalido_LanzaExcepcion(string? nombre)
        {
            Assert.Throws<PanelInvalidoException>(() => CrearPanel(nombre: nombre!));
        }

        [Fact]
        public void CrearPanel_ConNombreDemasiadoLargo_LanzaExcepcion()
        {
            var nombre = new string('a', Panel.LongitudMaximaNombre + 1);

            Assert.Throws<PanelInvalidoException>(() => CrearPanel(nombre: nombre));
        }

        [Fact]
        public void CrearPanel_ConFechaPorDefecto_LanzaExcepcion()
        {
            Assert.Throws<PanelInvalidoException>(() => CrearPanel(fechaCreacion: default(DateTime)));
        }

        [Fact]
        public void CrearPanel_ConFechaFutura_LanzaExcepcion()
        {
            Assert.Throws<PanelInvalidoException>(
                () => CrearPanel(fechaCreacion: DateTime.UtcNow.AddDays(1)));
        }

        [Fact]
        public void CrearPanel_ConEstadoIndefinido_LanzaExcepcion()
        {
            Assert.Throws<PanelInvalidoException>(() => CrearPanel(estado: (EstadoPanel)99));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        public void CrearPanel_ConPotenciaNoPositiva_LanzaExcepcion(decimal potencia)
        {
            Assert.Throws<PanelInvalidoException>(() => CrearPanel(potencia: potencia));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void CrearPanel_ConVidaUtilInvalida_LanzaExcepcion(string? vidaUtil)
        {
            Assert.Throws<PanelInvalidoException>(() => CrearPanel(vidaUtil: vidaUtil!));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-3)]
        public void CrearPanel_ConNodoInvalido_LanzaExcepcion(int idNodo)
        {
            Assert.Throws<PanelInvalidoException>(() => CrearPanel(idNodo: idNodo));
        }

        [Fact]
        public void PuedeCambiarA_ConPanelApagadoYAccionEncender_DevuelveTrue()
        {
            var panel = CrearPanel(estado: EstadoPanel.Apagado, encendido: false);

            Assert.True(panel.PuedeCambiarA(AccionEstadoPanel.Encender));
        }

        [Fact]
        public void PuedeCambiarA_ConPanelEncendidoYAccionApagar_DevuelveTrue()
        {
            var panel = CrearPanel(estado: EstadoPanel.Encendido, encendido: true);

            Assert.True(panel.PuedeCambiarA(AccionEstadoPanel.Apagar));
        }

        [Theory]
        [InlineData(EstadoPanel.Encendido, AccionEstadoPanel.Encender)]
        [InlineData(EstadoPanel.Apagado, AccionEstadoPanel.Apagar)]
        public void PuedeCambiarA_ConPanelYaEnElEstadoSolicitado_DevuelveFalse(
            EstadoPanel estado,
            AccionEstadoPanel accion)
        {
            var panel = CrearPanel(estado: estado, encendido: estado == EstadoPanel.Encendido);

            Assert.False(panel.PuedeCambiarA(accion));
        }

        [Fact]
        public void PuedeCambiarA_ConAccionIndefinida_LanzaExcepcion()
        {
            var panel = CrearPanel();

            Assert.Throws<PanelInvalidoException>(
                () => panel.PuedeCambiarA((AccionEstadoPanel)99));
        }

        [Theory]
        [InlineData(false, true)]
        [InlineData(true, false)]
        public void PuedeCambiarVolcadoEnergia_ConValorDiferente_DevuelveTrue(
            bool volcarEnergiaActual,
            bool volcarEnergiaSolicitado)
        {
            var panel = CrearPanel(volcarEnergia: volcarEnergiaActual);

            Assert.True(panel.PuedeCambiarVolcadoEnergia(volcarEnergiaSolicitado));
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void PuedeCambiarVolcadoEnergia_ConElMismoValor_DevuelveFalse(bool volcarEnergia)
        {
            var panel = CrearPanel(volcarEnergia: volcarEnergia);

            Assert.False(panel.PuedeCambiarVolcadoEnergia(volcarEnergia));
        }

        private static Panel CrearPanel(
            int id = 1,
            string nombre = "Panel Norte",
            DateTime? fechaCreacion = null,
            EstadoPanel estado = EstadoPanel.Encendido,
            bool encendido = true,
            decimal potencia = 450m,
            string vidaUtil = "25 anos",
            int idNodo = 1,
            bool volcarEnergia = false)
        {
            return new Panel(
                id,
                nombre,
                fechaCreacion ?? DateTime.UtcNow.AddDays(-1),
                estado,
                encendido,
                potencia,
                vidaUtil,
                idNodo,
                volcarEnergia);
        }
    }
}
