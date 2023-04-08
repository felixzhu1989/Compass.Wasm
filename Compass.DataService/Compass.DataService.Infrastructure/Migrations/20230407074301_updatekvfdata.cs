using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Compass.DataService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatekvfdata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SupplySpigotDistance",
                table: "ModulesData");

            migrationBuilder.AlterColumn<int>(
                name: "SupplySpigotNumber",
                table: "ModulesData",
                type: "int",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SupplySpigotDis",
                table: "ModulesData",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SupplySpigotDis",
                table: "ModulesData");

            migrationBuilder.AlterColumn<double>(
                name: "SupplySpigotNumber",
                table: "ModulesData",
                type: "float",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SupplySpigotDistance",
                table: "ModulesData",
                type: "float",
                nullable: true);
        }
    }
}
