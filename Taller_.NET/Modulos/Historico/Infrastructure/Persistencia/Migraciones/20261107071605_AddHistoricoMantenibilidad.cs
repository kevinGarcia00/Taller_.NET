using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taller_.NET.Modulos.Historico.Infrastructure.Persistencia.Migraciones
{
    /// <inheritdoc />
    public partial class AddHistoricoMantenibilidad : Migration
    {
        private static readonly string[] ColumnasPanelFecha = ["panel_id", "fecha"];

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HistoricosMantenibilidad",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    id_mantenedor = table.Column<Guid>(type: "TEXT", nullable: false),
                    fecha = table.Column<DateTime>(type: "TEXT", nullable: false),
                    observaciones = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    estado_inicial = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    estado_resultante = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    panel_id = table.Column<int>(type: "INTEGER", nullable: false),
                    nodo_id = table.Column<int>(type: "INTEGER", nullable: false),
                    parque_solar_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricosMantenibilidad", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_historicos_mantenibilidad_panel_fecha",
                table: "HistoricosMantenibilidad",
                columns: ColumnasPanelFecha);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HistoricosMantenibilidad");
        }
    }
}
