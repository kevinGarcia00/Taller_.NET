using System.Collections.Generic;
using Taller_.NET.Modulos.ParqueSolar.Application.Dtos;

namespace Taller_.NET.Modulos.ParqueSolar.Application.Puertos
{
    public interface IMediadorParqueNodo
    {
        IReadOnlyList<NodoResumenDto> ObtenerNodosPorParque(int parqueSolarId);
    }
}
