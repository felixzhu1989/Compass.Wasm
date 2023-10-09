using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Compass.ProjectService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addcutlistbendingmark : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BendingMark",
                table: "CutLists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BendingMark",
                table: "CutLists");
        }
    }
}
