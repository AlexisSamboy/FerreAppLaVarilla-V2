using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FerreAppLaVarilla.UI.Migrations
{
    /// <inheritdoc />
    public partial class AjustesModelosPOS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Camiones_CamionAsignadoId",
                table: "Pedidos");

            migrationBuilder.AlterColumn<int>(
                name: "CamionAsignadoId",
                table: "Pedidos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Camiones_CamionAsignadoId",
                table: "Pedidos",
                column: "CamionAsignadoId",
                principalTable: "Camiones",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Camiones_CamionAsignadoId",
                table: "Pedidos");

            migrationBuilder.AlterColumn<int>(
                name: "CamionAsignadoId",
                table: "Pedidos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Camiones_CamionAsignadoId",
                table: "Pedidos",
                column: "CamionAsignadoId",
                principalTable: "Camiones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
