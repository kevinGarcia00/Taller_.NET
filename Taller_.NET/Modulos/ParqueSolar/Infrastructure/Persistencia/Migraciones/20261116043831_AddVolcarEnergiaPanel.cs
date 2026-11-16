using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taller_.NET.Modulos.ParqueSolar.Infrastructure.Persistencia.Migraciones
{
    /// <inheritdoc />
    public partial class AddVolcarEnergiaPanel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "volcar_energia",
                table: "Paneles",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "volcar_energia",
                table: "Paneles");
        }
    }
}
