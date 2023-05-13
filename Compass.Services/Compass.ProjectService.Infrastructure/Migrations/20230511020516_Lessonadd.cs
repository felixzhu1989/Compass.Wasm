using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Compass.ProjectService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Lessonadd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Issues");

            migrationBuilder.CreateTable(
                name: "Lessons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lessons", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_Id_IsDeleted",
                table: "Lessons",
                columns: new[] { "Id", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_ProjectId_IsDeleted",
                table: "Lessons",
                columns: new[] { "ProjectId", "IsDeleted" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lessons");

            migrationBuilder.CreateTable(
                name: "Issues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectStatus = table.Column<int>(type: "int", nullable: false),
                    ReportUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Stakeholder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Issues", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Issues_Id_IsDeleted",
                table: "Issues",
                columns: new[] { "Id", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Issues_ProjectId_IsDeleted",
                table: "Issues",
                columns: new[] { "ProjectId", "IsDeleted" });
        }
    }
}
