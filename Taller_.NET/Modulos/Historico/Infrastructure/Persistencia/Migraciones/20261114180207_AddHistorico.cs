using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taller_.NET.Modulos.Historico.Infrastructure.Persistencia.Migraciones
{
    /// <inheritdoc />
    public partial class AddHistorico : Migration
    {
        private static readonly string[] ColumnasIndicePanelFecha = ["panel_id", "fecha"];

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Registros",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    fecha = table.Column<DateTime>(type: "TEXT", nullable: false),
                    energia_total = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    panel_id = table.Column<int>(type: "INTEGER", nullable: false),
                    nodo_id = table.Column<int>(type: "INTEGER", nullable: false),
                    parque_solar_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registros", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_registros_panel_fecha",
                table: "Registros",
                columns: ColumnasIndicePanelFecha);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Registros");
        }
    }
}
