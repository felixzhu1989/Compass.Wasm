using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Compass.DataService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addbaffle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "BaffleLeft",
                table: "ModulesData",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "BaffleM",
                table: "ModulesData",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BaffleMNumber",
                table: "ModulesData",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "BaffleRight",
                table: "ModulesData",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "BaffleW",
                table: "ModulesData",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaffleLeft",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "BaffleM",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "BaffleMNumber",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "BaffleRight",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "BaffleW",
                table: "ModulesData");
        }
    }
}
