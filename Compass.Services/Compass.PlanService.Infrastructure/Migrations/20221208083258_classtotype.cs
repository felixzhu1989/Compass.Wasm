using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Compass.PlanService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class classtotype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductionPlanClass",
                table: "ProductionPlans",
                newName: "ProductionPlanType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductionPlanType",
                table: "ProductionPlans",
                newName: "ProductionPlanClass");
        }
    }
}
