using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Compass.DataService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateuvfanduvi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BlueTooth",
                table: "ModulesData",
                newName: "Bluetooth");

            migrationBuilder.AddColumn<double>(
                name: "LightToFront",
                table: "ModulesData",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LightToFront",
                table: "ModulesData");

            migrationBuilder.RenameColumn(
                name: "Bluetooth",
                table: "ModulesData",
                newName: "BlueTooth");
        }
    }
}
