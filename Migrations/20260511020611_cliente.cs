using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FerreAppLaVarilla.UI.Migrations
{
    /// <inheritdoc />
    public partial class cliente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<string>(
              //  name: "CorreoElectronico",
                //table: "Clientes",
                //type: "nvarchar(max)",
                //nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorreoElectronico",
                table: "Clientes");
        }
    }
}
