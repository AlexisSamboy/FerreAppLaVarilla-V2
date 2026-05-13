using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FerreAppLaVarilla.UI.Migrations
{
    /// <inheritdoc />
    public partial class SincronizacionCamposUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Correo",
                table: "Usuarios");

            migrationBuilder.RenameColumn(
                name: "Nombre",
                table: "Usuarios",
                newName: "CorreoElectronico");

            migrationBuilder.AddColumn<string>(
                name: "CedulaRNC",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DireccionEntrega",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NombreCompleto",
                table: "Usuarios",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Telefono",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsuarioId",
                table: "Pedidos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_UsuarioId",
                table: "Pedidos",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Usuarios_UsuarioId",
                table: "Pedidos",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Usuarios_UsuarioId",
                table: "Pedidos");

            migrationBuilder.DropIndex(
                name: "IX_Pedidos_UsuarioId",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "CedulaRNC",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "DireccionEntrega",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "NombreCompleto",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Telefono",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Pedidos");

            migrationBuilder.RenameColumn(
                name: "CorreoElectronico",
                table: "Usuarios",
                newName: "Nombre");

            migrationBuilder.AddColumn<string>(
                name: "Correo",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
