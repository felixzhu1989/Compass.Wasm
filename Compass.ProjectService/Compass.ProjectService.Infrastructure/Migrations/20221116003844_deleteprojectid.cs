using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Compass.ProjectService.Infrastructure.Migrations
{
    public partial class deleteprojectid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Trackings_ProjectId_IsDeleted",
                table: "Trackings");

            migrationBuilder.DropIndex(
                name: "IX_DrawingsPlan_ProjectId_IsDeleted",
                table: "DrawingsPlan");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Trackings");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "DrawingsPlan");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "Trackings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "DrawingsPlan",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Trackings_ProjectId_IsDeleted",
                table: "Trackings",
                columns: new[] { "ProjectId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_DrawingsPlan_ProjectId_IsDeleted",
                table: "DrawingsPlan",
                columns: new[] { "ProjectId", "IsDeleted" });
        }
    }
}
