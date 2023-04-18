using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Compass.PlanService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class productplan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductionPlans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OdpReleaseTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SqNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ModelSummary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductionFinishTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DrawingReleaseTarget = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DrawingReleaseActual = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MonthOfInvoice = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProductionPlanClass = table.Column<int>(type: "int", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionPlans", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductionPlans_Id_IsDeleted",
                table: "ProductionPlans",
                columns: new[] { "Id", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductionPlans_ProjectId_IsDeleted",
                table: "ProductionPlans",
                columns: new[] { "ProjectId", "IsDeleted" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductionPlans");
        }
    }
}
