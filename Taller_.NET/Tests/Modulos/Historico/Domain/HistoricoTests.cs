using Taller_.NET.Modulos.Historico.Domain;
using Taller_.NET.Modulos.Historico.Domain.Excepciones;
using Xunit;

// El namespace de los tests "Taller_.NET.Tests.Modulos.Historico" tiene prioridad
// sobre el tipo "Historico", por lo que se usa un alias para referenciar la entidad.
using EntidadHistorico = Taller_.NET.Modulos.Historico.Domain.Historico;

namespace Taller_.NET.Tests.Modulos.Historico.Domain;

public class HistoricoTests
{
    [Fact]
    public void CrearHistorico_ConDatosValidos_AsignaPropiedades()
    {
        var fecha = DateTime.UtcNow;
        var contexto = new ContextoPanel(3, 2, 1);

        var registro = new EntidadHistorico(fecha, 123.45m, contexto);

        Assert.Equal(fecha, registro.Fecha);
        Assert.Equal(123.45m, registro.EnergiaTotal);
        Assert.Same(contexto, registro.ContextoPanel);
        Assert.Equal(0, registro.Id);
    }

    [Fact]
    public void CrearHistorico_ConEnergiaTotalCero_EsValido()
    {
        var registro = new EntidadHistorico(DateTime.UtcNow, 0m, new ContextoPanel(3, 2, 1));

        Assert.Equal(0m, registro.EnergiaTotal);
    }

    [Theory]
    [InlineData(-0.01)]
    [InlineData(-100)]
    public void CrearHistorico_ConEnergiaTotalNegativa_LanzaExcepcion(double energiaTotal)
    {
        Assert.Throws<HistoricoInvalidoException>(
            () => new EntidadHistorico(DateTime.UtcNow, (decimal)energiaTotal, new ContextoPanel(3, 2, 1)));
    }

    [Fact]
    public void CrearHistorico_ConFechaFutura_LanzaExcepcion()
    {
        var fechaFutura = DateTime.UtcNow.AddMinutes(5);

        Assert.Throws<HistoricoInvalidoException>(
            () => new EntidadHistorico(fechaFutura, 10m, new ContextoPanel(3, 2, 1)));
    }

    [Fact]
    public void CrearHistorico_ConContextoPanelNulo_LanzaExcepcion()
    {
        Assert.Throws<HistoricoInvalidoException>(
            () => new EntidadHistorico(DateTime.UtcNow, 10m, null!));
    }
}
