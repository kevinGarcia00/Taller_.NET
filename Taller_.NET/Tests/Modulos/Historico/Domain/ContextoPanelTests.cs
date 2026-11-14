using Taller_.NET.Modulos.Historico.Domain;
using Taller_.NET.Modulos.Historico.Domain.Excepciones;
using Xunit;

namespace Taller_.NET.Tests.Modulos.Historico.Domain;

public class ContextoPanelTests
{
    [Fact]
    public void CrearContextoPanel_ConIdsValidos_AsignaPropiedades()
    {
        var contexto = new ContextoPanel(10, 20, 30);

        Assert.Equal(10, contexto.PanelId);
        Assert.Equal(20, contexto.NodoId);
        Assert.Equal(30, contexto.ParqueSolarId);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void CrearContextoPanel_ConPanelIdInvalido_LanzaExcepcion(int panelId)
    {
        Assert.Throws<HistoricoInvalidoException>(() => new ContextoPanel(panelId, 20, 30));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void CrearContextoPanel_ConNodoIdInvalido_LanzaExcepcion(int nodoId)
    {
        Assert.Throws<HistoricoInvalidoException>(() => new ContextoPanel(10, nodoId, 30));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void CrearContextoPanel_ConParqueSolarIdInvalido_LanzaExcepcion(int parqueSolarId)
    {
        Assert.Throws<HistoricoInvalidoException>(() => new ContextoPanel(10, 20, parqueSolarId));
    }
}
