using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Compass.CategoryService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class materialitemsName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MaterialItem",
                table: "MaterialItem");

            migrationBuilder.RenameTable(
                name: "MaterialItem",
                newName: "MaterialItems");

            migrationBuilder.RenameIndex(
                name: "IX_MaterialItem_Id_IsDeleted",
                table: "MaterialItems",
                newName: "IX_MaterialItems_Id_IsDeleted");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MaterialItems",
                table: "MaterialItems",
                column: "Id")
                .Annotation("SqlServer:Clustered", false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MaterialItems",
                table: "MaterialItems");

            migrationBuilder.RenameTable(
                name: "MaterialItems",
                newName: "MaterialItem");

            migrationBuilder.RenameIndex(
                name: "IX_MaterialItems_Id_IsDeleted",
                table: "MaterialItem",
                newName: "IX_MaterialItem_Id_IsDeleted");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MaterialItem",
                table: "MaterialItem",
                column: "Id")
                .Annotation("SqlServer:Clustered", false);
        }
    }
}
