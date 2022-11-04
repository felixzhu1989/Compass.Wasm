using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Compass.ProjectService.Infrastructure.Migrations
{
    public partial class addmodule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ModelId",
                table: "Modules",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Modules",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Modules_DrawingId_IsDeleted",
                table: "Modules",
                columns: new[] { "DrawingId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Drawings_ProjectId_IsDeleted",
                table: "Drawings",
                columns: new[] { "ProjectId", "IsDeleted" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Modules_DrawingId_IsDeleted",
                table: "Modules");

            migrationBuilder.DropIndex(
                name: "IX_Drawings_ProjectId_IsDeleted",
                table: "Drawings");

            migrationBuilder.DropColumn(
                name: "ModelId",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Modules");
        }
    }
}
