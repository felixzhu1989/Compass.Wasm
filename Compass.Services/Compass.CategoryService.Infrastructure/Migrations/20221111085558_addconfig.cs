using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Compass.CategoryService.Infrastructure.Migrations
{
    public partial class addconfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_IssueTypes",
                table: "IssueTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IssueTypes",
                table: "IssueTypes",
                column: "Id")
                .Annotation("SqlServer:Clustered", true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_IssueTypes",
                table: "IssueTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IssueTypes",
                table: "IssueTypes",
                column: "Id");
        }
    }
}
