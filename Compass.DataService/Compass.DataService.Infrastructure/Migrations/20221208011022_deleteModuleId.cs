using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Compass.DataService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class deleteModuleId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ModuleData",
                table: "ModuleData");

            migrationBuilder.DropColumn(
                name: "ModuleId",
                table: "ModuleData");

            migrationBuilder.RenameTable(
                name: "ModuleData",
                newName: "ModulesData");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ModulesData",
                table: "ModulesData",
                column: "Id")
                .Annotation("SqlServer:Clustered", false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ModulesData",
                table: "ModulesData");

            migrationBuilder.RenameTable(
                name: "ModulesData",
                newName: "ModuleData");

            migrationBuilder.AddColumn<Guid>(
                name: "ModuleId",
                table: "ModuleData",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ModuleData",
                table: "ModuleData",
                column: "Id")
                .Annotation("SqlServer:Clustered", false);
        }
    }
}
