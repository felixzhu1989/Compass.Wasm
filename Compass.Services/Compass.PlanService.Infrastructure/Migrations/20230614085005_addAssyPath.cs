using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Compass.PlanService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addAssyPath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssyPath",
                table: "PackingLists",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssyPath",
                table: "PackingLists");
        }
    }
}
