using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Compass.DataService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addtag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ansul",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "AnsulDetector",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "AnsulDrop",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "AnsulSide",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "BackCj",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "BackToBack",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "BlueTooth",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "CoverBoard",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "DrainType",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "ExhaustSpigotHeight",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "ExhaustSpigotLength",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "ExhaustSpigotNumber",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "ExhaustSpigotWidth",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "LedLogo",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "LightType",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "Marvel",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "MiddleToRight",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "SidePanel",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "SpotLightDistance",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "SpotLightNumber",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "SupplySpigotDistance",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "SupplySpigotNumber",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "UvLightType",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "WaterCollection",
                table: "ModulesData");

            migrationBuilder.RenameColumn(
                name: "Discriminator",
                table: "ModulesData",
                newName: "Tag");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Tag",
                table: "ModulesData",
                newName: "Discriminator");

            migrationBuilder.AddColumn<bool>(
                name: "Ansul",
                table: "ModulesData",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AnsulDetector",
                table: "ModulesData",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AnsulDrop",
                table: "ModulesData",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AnsulSide",
                table: "ModulesData",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "BackCj",
                table: "ModulesData",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "BackToBack",
                table: "ModulesData",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "BlueTooth",
                table: "ModulesData",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CoverBoard",
                table: "ModulesData",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DrainType",
                table: "ModulesData",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ExhaustSpigotHeight",
                table: "ModulesData",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ExhaustSpigotLength",
                table: "ModulesData",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExhaustSpigotNumber",
                table: "ModulesData",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ExhaustSpigotWidth",
                table: "ModulesData",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "LedLogo",
                table: "ModulesData",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LightType",
                table: "ModulesData",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Marvel",
                table: "ModulesData",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MiddleToRight",
                table: "ModulesData",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SidePanel",
                table: "ModulesData",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SpotLightDistance",
                table: "ModulesData",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SpotLightNumber",
                table: "ModulesData",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SupplySpigotDistance",
                table: "ModulesData",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SupplySpigotNumber",
                table: "ModulesData",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UvLightType",
                table: "ModulesData",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "WaterCollection",
                table: "ModulesData",
                type: "bit",
                nullable: true);
        }
    }
}
