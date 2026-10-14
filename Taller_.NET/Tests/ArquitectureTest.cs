using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Fluent.Slices;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
using Taller_.NET.Modulos.Nodo;
using Xunit;

namespace Taller_.NET.Tests;

public class ArquitectureTest
{
    private const string PatronModulos = "Taller_.NET.Modulos.(*)..";

    private static readonly Architecture Architecture =
        new ArchLoader().LoadAssemblies(typeof(Nodo).Assembly).Build();

    [Fact]
    public void ModulosNoDebenDependerEntreSi()
    {
        IArchRule regla = SliceRuleDefinition.Slices()
            .Matching(PatronModulos)
            .Should()
            .NotDependOnEachOther();

        regla.Check(Architecture);
    }

    [Fact]
    public void ModulosNoDebenTenerDependenciasCiclicas()
    {
        IArchRule regla = SliceRuleDefinition.Slices()
            .Matching(PatronModulos)
            .Should()
            .BeFreeOfCycles();

        regla.Check(Architecture);
    }
}
