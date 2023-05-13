using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Compass.ProjectService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class projectremoveissue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Issues");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Issues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CloseTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deadline = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsClosed = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReporterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResponderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Solution = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SolutionUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
