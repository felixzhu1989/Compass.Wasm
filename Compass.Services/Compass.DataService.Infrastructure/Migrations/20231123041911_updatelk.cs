using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Compass.DataService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatelk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "LeftLength",
                table: "ModulesData",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LongGlassNumber",
                table: "ModulesData",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MiddleLength",
                table: "ModulesData",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "RightLength",
                table: "ModulesData",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ShortGlassNumber",
                table: "ModulesData",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeftLength",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "LongGlassNumber",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "MiddleLength",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "RightLength",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "ShortGlassNumber",
                table: "ModulesData");
        }
    }
}
