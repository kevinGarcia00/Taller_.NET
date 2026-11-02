using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Taller_.NET.Modulos.Usuario.Infrastructure.Persistencia.Migraciones
{
    /// <inheritdoc />
    public partial class EliminarRolesLocales : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsuarioRoles");

            migrationBuilder.DropTable(
                name: "Roles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nombre = table.Column<string>(type: "TEXT", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioRoles",
                columns: table => new
                {
                    RolesId = table.Column<long>(type: "INTEGER", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioRoles", x => new { x.RolesId, x.UsuarioId });
                    table.ForeignKey(
                        name: "FK_UsuarioRoles_Roles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "Roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioRoles_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "id", "nombre" },
                values: new object[,]
                {
                    { 1L, "Administrador" },
                    { 2L, "Mantenimiento" }
                });

            migrationBuilder.CreateIndex(
                name: "ux_roles_nombre",
                table: "Roles",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioRoles_UsuarioId",
                table: "UsuarioRoles",
                column: "UsuarioId");
        }
    }
}
