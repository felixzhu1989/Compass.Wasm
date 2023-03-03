using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Compass.DataService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class kviupdateansul : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AnsulDropDis1",
                table: "ModulesData",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "AnsulDropDis2",
                table: "ModulesData",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "AnsulDropDis3",
                table: "ModulesData",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "AnsulDropDis4",
                table: "ModulesData",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "AnsulDropDis5",
                table: "ModulesData",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "AnsulDropToFront",
                table: "ModulesData",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnsulDropDis1",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "AnsulDropDis2",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "AnsulDropDis3",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "AnsulDropDis4",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "AnsulDropDis5",
                table: "ModulesData");

            migrationBuilder.DropColumn(
                name: "AnsulDropToFront",
                table: "ModulesData");
        }
    }
}
