using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Compass.DataService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatecjnocj : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NocjBackSide",
                table: "ModulesData",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NocjSide",
                table: "ModulesData",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NocjBackSide",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "NocjSide",
                table: "ModulesData");
        }
    }
}
