using System;

namespace Taller_.NET.Modulos.ParqueSolar.Application.Excepciones
{
    public class ParqueConNodosAsociadosException : Exception
    {
        public ParqueConNodosAsociadosException(int parqueSolarId)
            : base($"El parque solar con el identificador {parqueSolarId} tiene nodos asociados y no puede eliminarse.")
        {
        }
    }
}
