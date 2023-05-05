using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Compass.PlanService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mainplanchangename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SqNumber",
                table: "MainPlans",
                newName: "Number");

            migrationBuilder.RenameColumn(
                name: "ProductionFinishTime",
                table: "MainPlans",
                newName: "FinishTime");

            migrationBuilder.RenameColumn(
                name: "OdpReleaseTime",
                table: "MainPlans",
                newName: "DrwReleaseTarget");

            migrationBuilder.RenameColumn(
                name: "DrawingReleaseTarget",
                table: "MainPlans",
                newName: "CreateTime");

            migrationBuilder.RenameColumn(
                name: "DrawingReleaseActual",
                table: "MainPlans",
                newName: "DrwReleaseActual");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Number",
                table: "MainPlans",
                newName: "SqNumber");

            migrationBuilder.RenameColumn(
                name: "FinishTime",
                table: "MainPlans",
                newName: "ProductionFinishTime");

            migrationBuilder.RenameColumn(
                name: "DrwReleaseTarget",
                table: "MainPlans",
                newName: "OdpReleaseTime");

            migrationBuilder.RenameColumn(
                name: "DrwReleaseActual",
                table: "MainPlans",
                newName: "DrawingReleaseActual");

            migrationBuilder.RenameColumn(
                name: "CreateTime",
                table: "MainPlans",
                newName: "DrawingReleaseTarget");
        }
    }
}
