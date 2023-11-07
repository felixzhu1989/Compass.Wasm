using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Compass.DataService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class adddpback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DpBackSide",
                table: "ModulesData",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DpDrainType",
                table: "ModulesData",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DpBackSide",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "DpDrainType",
                table: "ModulesData");
        }
    }
}
