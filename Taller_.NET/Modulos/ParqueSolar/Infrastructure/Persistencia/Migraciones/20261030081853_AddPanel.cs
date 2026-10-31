using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taller_.NET.Modulos.ParqueSolar.Infrastructure.Persistencia.Migraciones
{
    /// <inheritdoc />
    public partial class AddPanel : Migration
    {
        private static readonly string[] ColumnasIndiceNombreNodo = ["nombre", "nodo_id"];

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Paneles",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nombre = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "TEXT", nullable: false),
                    estado = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    potencia = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    vida_util = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    nodo_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paneles", x => x.id);
                    table.ForeignKey(
                        name: "FK_Paneles_Nodos_nodo_id",
                        column: x => x.nodo_id,
                        principalTable: "Nodos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Paneles_nodo_id",
                table: "Paneles",
                column: "nodo_id");

            migrationBuilder.CreateIndex(
                name: "ux_paneles_nombre_nodo",
                table: "Paneles",
                columns: ColumnasIndiceNombreNodo,
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Paneles");
        }
    }
}
