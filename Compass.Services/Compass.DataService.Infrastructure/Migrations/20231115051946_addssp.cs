using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Compass.DataService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addssp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "LedLight",
                table: "ModulesData",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LeftType",
                table: "ModulesData",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "LeftWidth",
                table: "ModulesData",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MPanelNumber",
                table: "ModulesData",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RightType",
                table: "ModulesData",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "RightWidth",
                table: "ModulesData",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LedLight",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "LeftType",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "LeftWidth",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "MPanelNumber",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "RightType",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "RightWidth",
                table: "ModulesData");
        }
    }
}
