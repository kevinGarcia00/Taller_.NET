using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taller_.NET.Modulos.ParqueSolar.Infrastructure.Persistencia.Migraciones
{
    /// <inheritdoc />
    public partial class AddNodosEInversores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Inversores",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nombre = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inversores", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Nodos",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nombre = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    ubicacion_parque = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "TEXT", nullable: false),
                    energia_generada = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    inversor_id = table.Column<int>(type: "INTEGER", nullable: true),
                    parque_solar_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nodos", x => x.id);
                    table.ForeignKey(
                        name: "FK_Nodos_Inversores_inversor_id",
                        column: x => x.inversor_id,
                        principalTable: "Inversores",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "ux_nodos_inversor_id",
                table: "Nodos",
                column: "inversor_id",
                unique: true,
                filter: "inversor_id IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ux_nodos_nombre_parque",
                table: "Nodos",
                columns: new[] { "nombre", "parque_solar_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Nodos");

            migrationBuilder.DropTable(
                name: "Inversores");
        }
    }
}
