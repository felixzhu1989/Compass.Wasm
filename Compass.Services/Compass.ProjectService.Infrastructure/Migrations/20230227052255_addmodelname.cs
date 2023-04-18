using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Compass.ProjectService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addmodelname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectStatus",
                table: "Trackings");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Drawings");

            migrationBuilder.RenameColumn(
                name: "ReleaseTime",
                table: "DrawingsPlan",
                newName: "AssignTime");

            migrationBuilder.AddColumn<string>(
                name: "ModelName",
                table: "Modules",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "DrawingsPlan",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModelName",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "DrawingsPlan");

            migrationBuilder.RenameColumn(
                name: "AssignTime",
                table: "DrawingsPlan",
                newName: "ReleaseTime");

            migrationBuilder.AddColumn<int>(
                name: "ProjectStatus",
                table: "Trackings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Drawings",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}
