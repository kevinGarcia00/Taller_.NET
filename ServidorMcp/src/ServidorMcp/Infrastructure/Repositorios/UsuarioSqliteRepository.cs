using Application.Dtos;
using Application.Puertos;
using Infrastructure.Configuracion;
using Microsoft.Data.Sqlite;

namespace Infrastructure.Repositorios;

public sealed class UsuarioSqliteRepository : IUsuarioConsultaRepository
{
    private readonly string _cadenaConexion;

    public UsuarioSqliteRepository(OpcionesBaseDatosSolarUy opcionesBaseDatos)
    {
        ArgumentNullException.ThrowIfNull(opcionesBaseDatos);

        _cadenaConexion = new SqliteConnectionStringBuilder
        {
            DataSource = opcionesBaseDatos.RutaUsuarios,
            Mode = SqliteOpenMode.ReadOnly
        }.ToString();
    }

    public async Task<IReadOnlyList<UsuarioListadoDto>> ObtenerUsuariosAsync(
        CancellationToken cancellationToken = default)
    {
        await using var conexion = new SqliteConnection(_cadenaConexion);
        await conexion.OpenAsync(cancellationToken);

        await using var comando = conexion.CreateCommand();
        comando.CommandText = """
            SELECT id, nombre, apellido, correo, telefono, auth0_user_id
            FROM Usuarios
            ORDER BY nombre, apellido;
            """;

        var usuarios = new List<UsuarioListadoDto>();
        await using var lector = await comando.ExecuteReaderAsync(cancellationToken);

        while (await lector.ReadAsync(cancellationToken))
        {
            usuarios.Add(new UsuarioListadoDto
            {
                Id = Guid.Parse(lector.GetString(0)),
                Nombre = lector.GetString(1),
                Apellido = lector.GetString(2),
                Correo = lector.GetString(3),
                Telefono = lector.GetString(4),
                Auth0UserId = lector.IsDBNull(5) ? null : lector.GetString(5)
            });
        }

        return usuarios;
    }
}