using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Compass.DataService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addkvw : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CeilingLightType",
                table: "ModulesData",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DomeSsp",
                table: "ModulesData",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FilterBlindNumber",
                table: "ModulesData",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FilterLeft",
                table: "ModulesData",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FilterRight",
                table: "ModulesData",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FilterSide",
                table: "ModulesData",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FilterType",
                table: "ModulesData",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Gutter",
                table: "ModulesData",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "GutterWidth",
                table: "ModulesData",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "HclLeft",
                table: "ModulesData",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "HclRight",
                table: "ModulesData",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HclSide",
                table: "ModulesData",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Japan",
                table: "ModulesData",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LightCable",
                table: "ModulesData",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CeilingLightType",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "DomeSsp",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "FilterBlindNumber",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "FilterLeft",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "FilterRight",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "FilterSide",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "FilterType",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "Gutter",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "GutterWidth",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "HclLeft",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "HclRight",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "HclSide",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "Japan",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "LightCable",
                table: "ModulesData");
        }
    }
}
