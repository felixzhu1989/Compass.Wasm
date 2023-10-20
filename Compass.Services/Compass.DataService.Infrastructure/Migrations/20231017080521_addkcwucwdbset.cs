using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Compass.DataService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addkcwucwdbset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "BaffleSensorDis1",
                table: "ModulesData",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "BaffleSensorDis2",
                table: "ModulesData",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BaffleSensorNumber",
                table: "ModulesData",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CeilingWaterInlet",
                table: "ModulesData",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DpSide",
                table: "ModulesData",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaffleSensorDis1",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "BaffleSensorDis2",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "BaffleSensorNumber",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "CeilingWaterInlet",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "DpSide",
                table: "ModulesData");
        }
    }
}
