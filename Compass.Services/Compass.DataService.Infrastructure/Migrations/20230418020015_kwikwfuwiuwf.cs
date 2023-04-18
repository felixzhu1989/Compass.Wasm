using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Compass.DataService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class kwikwfuwiuwf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AnsulDetectorDis1",
                table: "ModulesData",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "AnsulDetectorDis2",
                table: "ModulesData",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "AnsulDetectorDis3",
                table: "ModulesData",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "AnsulDetectorDis4",
                table: "ModulesData",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "AnsulDetectorDis5",
                table: "ModulesData",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AnsulDetectorEnd",
                table: "ModulesData",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AnsulDetectorNumber",
                table: "ModulesData",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WaterInlet",
                table: "ModulesData",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnsulDetectorDis1",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "AnsulDetectorDis2",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "AnsulDetectorDis3",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "AnsulDetectorDis4",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "AnsulDetectorDis5",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "AnsulDetectorEnd",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "AnsulDetectorNumber",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "WaterInlet",
                table: "ModulesData");
        }
    }
}
