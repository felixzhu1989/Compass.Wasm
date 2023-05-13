using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Compass.ProjectService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class projectdproblembool : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsProblemNotResolved",
                table: "Projects");

            migrationBuilder.AlterColumn<string>(
                name: "RiskLevel",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ProjectType",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "RiskLevel",
                table: "Projects",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectType",
                table: "Projects",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "IsProblemNotResolved",
                table: "Projects",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
