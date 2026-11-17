using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Taller_.NET.Modulos.ParqueSolar.Application.Dtos;
using Taller_.NET.Modulos.ParqueSolar.Application.Puertos;
using Taller_.NET.Modulos.ParqueSolar.Domain;
using Taller_.NET.Modulos.ParqueSolar.Infrastructure.Persistencia;

namespace Taller_.NET.Modulos.ParqueSolar.Infrastructure.Mediadores
{
    public class MediadorParqueNodo : IMediadorParqueNodo
    {
        private const string ConsultaNodos =
            "SELECT id, nombre, ubicacion_parque, fecha_creacion, energia_generada " +
            "FROM Nodos WHERE parque_solar_id = $parqueSolarId ORDER BY id";

        private readonly ParqueDbContext _contexto;

        public MediadorParqueNodo(ParqueDbContext contexto)
        {
            _contexto = contexto;
        }

        public IReadOnlyList<NodoResumenDto> ObtenerNodosPorParque(int parqueSolarId)
        {
            var nodos = new List<NodoResumenDto>();

            using var comando = _contexto.Database.GetDbConnection().CreateCommand();
            comando.CommandText = ConsultaNodos;
            comando.Parameters.Add(new SqliteParameter("$parqueSolarId", parqueSolarId));

            AbrirConexionSiEsNecesario();

            try
            {
                using var lector = comando.ExecuteReader();

                while (lector.Read())
                {
                    nodos.Add(new NodoResumenDto
                    {
                        Id = lector.GetInt32(0),
                        Nombre = lector.GetString(1),
                        UbicacionParque = lector.GetString(2),
                        FechaCreacion = lector.GetDateTime(3),
                        EnergiaGenerada = lector.GetDecimal(4)
                    });
                }
            }
            catch (SqliteException)
            {
                return new List<NodoResumenDto>();
            }

            CalcularEnergiaGenerada(nodos);

            return nodos;
        }

        private void CalcularEnergiaGenerada(IReadOnlyCollection<NodoResumenDto> nodos)
        {
            if (nodos.Count == 0)
            {
                return;
            }

            var ids = nodos.Select(nodo => nodo.Id).ToHashSet();
            var energiaPorNodo = _contexto.Paneles
                .AsNoTracking()
                .Where(panel => ids.Contains(panel.NodoId))
                .ToList()
                .GroupBy(panel => panel.NodoId)
                .ToDictionary(
                    grupo => grupo.Key,
                    grupo => grupo.Sum(panel => panel.CalcularEnergiaAportada()));

            foreach (var nodo in nodos)
            {
                nodo.EnergiaGenerada = energiaPorNodo.GetValueOrDefault(nodo.Id);
            }
        }

        private void AbrirConexionSiEsNecesario()
        {
            var conexion = _contexto.Database.GetDbConnection();

            if (conexion.State != System.Data.ConnectionState.Open)
            {
                conexion.Open();
            }
        }
    }
}
