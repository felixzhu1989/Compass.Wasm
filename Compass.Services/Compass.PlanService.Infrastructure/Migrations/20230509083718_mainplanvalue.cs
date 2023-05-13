using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Compass.PlanService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mainplanvalue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ItemLine",
                table: "MainPlans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Value",
                table: "MainPlans",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Workload",
                table: "MainPlans",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemLine",
                table: "MainPlans");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "MainPlans");

            migrationBuilder.DropColumn(
                name: "Workload",
                table: "MainPlans");
        }
    }
}
