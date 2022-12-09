using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Compass.DataService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class adduvf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Bluetooth",
                table: "ModulesData",
                newName: "BlueTooth");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BlueTooth",
                table: "ModulesData",
                newName: "Bluetooth");
        }
    }
}
