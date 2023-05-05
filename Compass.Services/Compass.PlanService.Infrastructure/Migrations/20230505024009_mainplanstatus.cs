using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Compass.PlanService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mainplanstatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DrwReleaseActual",
                table: "MainPlans");

            migrationBuilder.RenameColumn(
                name: "ShippingStartTime",
                table: "MainPlans",
                newName: "ShippingTime");

            migrationBuilder.RenameColumn(
                name: "ShippingEndTime",
                table: "MainPlans",
                newName: "DrwReleaseTime");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "MainPlans",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "MainPlans");

            migrationBuilder.RenameColumn(
                name: "ShippingTime",
                table: "MainPlans",
                newName: "ShippingStartTime");

            migrationBuilder.RenameColumn(
                name: "DrwReleaseTime",
                table: "MainPlans",
                newName: "ShippingEndTime");

            migrationBuilder.AddColumn<DateTime>(
                name: "DrwReleaseActual",
                table: "MainPlans",
                type: "datetime2",
                nullable: true);
        }
    }
}
