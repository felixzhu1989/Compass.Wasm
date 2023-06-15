using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Compass.CategoryService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class categorymaterialitem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MaterialItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MtlNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Inventory = table.Column<double>(type: "float", nullable: false),
                    UnitCost = table.Column<double>(type: "float", nullable: false),
                    Length = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Width = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Height = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Material = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Hood = table.Column<bool>(type: "bit", nullable: false),
                    HoodGroup = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ceiling = table.Column<bool>(type: "bit", nullable: false),
                    CeilingGroup = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CeilingRule = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialItem", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MaterialItem_Id_IsDeleted",
                table: "MaterialItem",
                columns: new[] { "Id", "IsDeleted" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaterialItem");
        }
    }
}
