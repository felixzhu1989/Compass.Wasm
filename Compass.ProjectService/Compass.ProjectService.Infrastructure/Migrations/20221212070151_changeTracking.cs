using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Compass.ProjectService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changeTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClosedTime",
                table: "Trackings");

            migrationBuilder.DropColumn(
                name: "DrawingPlanedTime",
                table: "Trackings");

            migrationBuilder.RenameColumn(
                name: "ShippingTime",
                table: "Trackings",
                newName: "ShippingStartTime");

            migrationBuilder.RenameColumn(
                name: "ModuleReleaseTime",
                table: "Trackings",
                newName: "ShippingEndTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShippingStartTime",
                table: "Trackings",
                newName: "ShippingTime");

            migrationBuilder.RenameColumn(
                name: "ShippingEndTime",
                table: "Trackings",
                newName: "ModuleReleaseTime");

            migrationBuilder.AddColumn<DateTime>(
                name: "ClosedTime",
                table: "Trackings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DrawingPlanedTime",
                table: "Trackings",
                type: "datetime2",
                nullable: true);
        }
    }
}
