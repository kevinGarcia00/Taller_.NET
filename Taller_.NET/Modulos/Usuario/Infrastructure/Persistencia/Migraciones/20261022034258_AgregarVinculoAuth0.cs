using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taller_.NET.Modulos.Usuario.Infrastructure.Persistencia.Migraciones
{
    /// <inheritdoc />
    public partial class AgregarVinculoAuth0 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "auth0_user_id",
                table: "Usuarios",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ux_usuarios_auth0_user_id",
                table: "Usuarios",
                column: "auth0_user_id",
                unique: true,
                filter: "auth0_user_id IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ux_usuarios_auth0_user_id",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "auth0_user_id",
                table: "Usuarios");
        }
    }
}
