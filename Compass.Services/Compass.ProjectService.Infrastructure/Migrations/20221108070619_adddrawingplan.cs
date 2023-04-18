using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Compass.ProjectService.Infrastructure.Migrations
{
    public partial class adddrawingplan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "OdpNumber",
                table: "Projects",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "DrawingsPlan",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReleaseTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrawingsPlan", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Id_IsDeleted",
                table: "Projects",
                columns: new[] { "Id", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_OdpNumber_IsDeleted",
                table: "Projects",
                columns: new[] { "OdpNumber", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Modules_Id_IsDeleted",
                table: "Modules",
                columns: new[] { "Id", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Drawings_Id_IsDeleted",
                table: "Drawings",
                columns: new[] { "Id", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_DrawingsPlan_Id_IsDeleted",
                table: "DrawingsPlan",
                columns: new[] { "Id", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_DrawingsPlan_ProjectId_IsDeleted",
                table: "DrawingsPlan",
                columns: new[] { "ProjectId", "IsDeleted" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DrawingsPlan");

            migrationBuilder.DropIndex(
                name: "IX_Projects_Id_IsDeleted",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_OdpNumber_IsDeleted",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Modules_Id_IsDeleted",
                table: "Modules");

            migrationBuilder.DropIndex(
                name: "IX_Drawings_Id_IsDeleted",
                table: "Drawings");

            migrationBuilder.AlterColumn<string>(
                name: "OdpNumber",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
