using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Compass.DataService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addcjdata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BcjSide",
                table: "ModulesData",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CjSpigotDirection",
                table: "ModulesData",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GutterSide",
                table: "ModulesData",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LeftBeamType",
                table: "ModulesData",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "LeftDbToRight",
                table: "ModulesData",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "LeftEndDis",
                table: "ModulesData",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "LeftGutterWidth",
                table: "ModulesData",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LksSide",
                table: "ModulesData",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RightBeamType",
                table: "ModulesData",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "RightDbToLeft",
                table: "ModulesData",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "RightEndDis",
                table: "ModulesData",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "RightGutterWidth",
                table: "ModulesData",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BcjSide",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "CjSpigotDirection",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "GutterSide",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "LeftBeamType",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "LeftDbToRight",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "LeftEndDis",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "LeftGutterWidth",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "LksSide",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "RightBeamType",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "RightDbToLeft",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "RightEndDis",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "RightGutterWidth",
                table: "ModulesData");
        }
    }
}
