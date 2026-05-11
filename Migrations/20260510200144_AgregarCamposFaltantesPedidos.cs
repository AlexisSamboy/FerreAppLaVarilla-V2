using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FerreAppLaVarilla.UI.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCamposFaltantesPedidos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "RequiereDelivery",
                table: "Pedidos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TipoDocumento",
                table: "Pedidos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequiereDelivery",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "TipoDocumento",
                table: "Pedidos");
        }
    }
}
