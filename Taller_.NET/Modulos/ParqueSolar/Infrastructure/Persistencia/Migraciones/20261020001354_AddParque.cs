using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taller_.NET.Modulos.ParqueSolar.Infrastructure.Persistencia.Migraciones
{
    /// <inheritdoc />
    public partial class AddParque : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Parques",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nombre = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    ubicacion = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "TEXT", nullable: false),
                    cant_max_nodos = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parques", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ux_parques_nombre",
                table: "Parques",
                column: "nombre",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Parques");
        }
    }
}
