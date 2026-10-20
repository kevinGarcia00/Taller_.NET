using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
using Taller_.NET.Modulos.Nodo;
using Xunit;

namespace Taller_.NET.Tests;

public class ArquitectureTest
{
    private const string PrefijoModulos = "Taller_.NET.Modulos.";

    private static readonly Architecture Architecture =
        new ArchLoader().LoadAssemblies(typeof(Nodo).Assembly).Build();

    [Fact]
    public void ModulosNoDebenDependerEntreSi()
    {
        var dependenciasEntreModulos = ObtenerDependenciasEntreModulos();

        Assert.True(
            dependenciasEntreModulos.Count == 0,
            $"Se detectaron dependencias directas entre modulos distintos: {string.Join(", ", dependenciasEntreModulos)}");
    }

    [Fact]
    public void ModulosNoDebenTenerDependenciasCiclicas()
    {
        var grafo = ObtenerDependenciasEntreModulos()
            .Select(dependencia => (Dependencia: dependencia, Origen: ObtenerModulo(dependencia), Destino: ObtenerModulo(dependencia, destino: true)))
            .ToList();

        var ciclo = BuscarCiclo(grafo);

        Assert.True(
            ciclo == null,
            $"Se detecto una dependencia ciclica entre modulos: {ciclo}");
    }

    private static List<string> ObtenerDependenciasEntreModulos()
    {
        var tiposPorModulo = ObtenerTiposPorModulo();

        var dependencias = new List<string>();

        foreach (var tipo in Architecture.Types)
        {
            var moduloOrigen = ObtenerModulo(tipo);

            if (moduloOrigen == null)
            {
                continue;
            }

            foreach (var dependencia in tipo.Dependencies)
            {
                var moduloDestino = ObtenerModulo(dependencia.Target);

                if (moduloDestino == null || moduloDestino == moduloOrigen)
                {
                    continue;
                }

                dependencias.Add($"{moduloOrigen} -> {moduloDestino}");
            }
        }

        return dependencias.Distinct().OrderBy(dependencia => dependencia).ToList();
    }

    private static Dictionary<string, List<string>> ObtenerTiposPorModulo()
    {
        var mapa = new Dictionary<string, List<string>>();

        foreach (var tipo in Architecture.Types)
        {
            var modulo = ObtenerModulo(tipo);

            if (modulo == null)
            {
                continue;
            }

            if (!mapa.TryGetValue(modulo, out var tipos))
            {
                tipos = new List<string>();
                mapa[modulo] = tipos;
            }

            tipos.Add(tipo.FullName);
        }

        return mapa;
    }

    private static string? ObtenerModulo(IType tipo)
    {
        var nombre = tipo.FullName;

        if (!nombre.StartsWith(PrefijoModulos, StringComparison.Ordinal))
        {
            return null;
        }

        var resto = nombre[PrefijoModulos.Length..];
        var separador = resto.IndexOf('.');

        return separador < 0 ? resto : resto[..separador];
    }

    private static string ObtenerModulo(string dependencia, bool destino = false)
    {
        var partes = dependencia.Split(" -> ");

        return destino ? partes[1] : partes[0];
    }

    private static string? BuscarCiclo(List<(string Dependencia, string Origen, string Destino)> grafo)
    {
        var adyacencia = grafo
            .GroupBy(arista => arista.Origen)
            .ToDictionary(grupo => grupo.Key, grupo => grupo.Select(arista => arista.Destino).Distinct().ToList());

        var visitados = new HashSet<string>();
        var enCamino = new HashSet<string>();

        foreach (var modulo in adyacencia.Keys)
        {
            var ciclo = BuscarCicloDesde(modulo, adyacencia, visitados, enCamino, new List<string>());

            if (ciclo != null)
            {
                return ciclo;
            }
        }

        return null;
    }

    private static string? BuscarCicloDesde(
        string modulo,
        Dictionary<string, List<string>> adyacencia,
        HashSet<string> visitados,
        HashSet<string> enCamino,
        List<string> camino)
    {
        if (enCamino.Contains(modulo))
        {
            return string.Join(" -> ", camino.Append(modulo));
        }

        if (!visitados.Add(modulo))
        {
            return null;
        }

        enCamino.Add(modulo);
        camino.Add(modulo);

        if (adyacencia.TryGetValue(modulo, out var vecinos))
        {
            foreach (var vecino in vecinos)
            {
                var ciclo = BuscarCicloDesde(vecino, adyacencia, visitados, enCamino, camino);

                if (ciclo != null)
                {
                    return ciclo;
                }
            }
        }

        enCamino.Remove(modulo);
        camino.RemoveAt(camino.Count - 1);

        return null;
    }
}
