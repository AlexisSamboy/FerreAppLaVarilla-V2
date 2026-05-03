using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FerreAppLaVarilla.UI.Migrations
{
    /// <inheritdoc />
    public partial class AgregarImagenAProducto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Stock",
                table: "Productos");

            migrationBuilder.RenameColumn(
                name: "Descripcion",
                table: "Productos",
                newName: "ImagenUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImagenUrl",
                table: "Productos",
                newName: "Descripcion");

            migrationBuilder.AddColumn<int>(
                name: "Stock",
                table: "Productos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
