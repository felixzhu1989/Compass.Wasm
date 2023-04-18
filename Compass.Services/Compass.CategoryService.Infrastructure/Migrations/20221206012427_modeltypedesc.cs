using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Compass.CategoryService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class modeltypedesc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ModelTypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "ModelTypes");
        }
    }
}
