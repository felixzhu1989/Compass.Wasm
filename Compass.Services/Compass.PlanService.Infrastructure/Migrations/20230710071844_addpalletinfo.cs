using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Compass.PlanService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addpalletinfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pallets");

            migrationBuilder.AddColumn<string>(
                name: "GrossWeight",
                table: "PackingItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NetWeight",
                table: "PackingItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PalletHeight",
                table: "PackingItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PalletLength",
                table: "PackingItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PalletNumber",
                table: "PackingItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PalletRemark",
                table: "PackingItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PalletWidth",
                table: "PackingItems",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GrossWeight",
                table: "PackingItems");

            migrationBuilder.DropColumn(
                name: "NetWeight",
                table: "PackingItems");

            migrationBuilder.DropColumn(
                name: "PalletHeight",
                table: "PackingItems");

            migrationBuilder.DropColumn(
                name: "PalletLength",
                table: "PackingItems");

            migrationBuilder.DropColumn(
                name: "PalletNumber",
                table: "PackingItems");

            migrationBuilder.DropColumn(
                name: "PalletRemark",
                table: "PackingItems");

            migrationBuilder.DropColumn(
                name: "PalletWidth",
                table: "PackingItems");

            migrationBuilder.CreateTable(
                name: "Pallets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GrossWeight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Height = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ItemHeight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemLength = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemWidth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Length = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NetWeight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PackingListId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PalletNumber = table.Column<int>(type: "int", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Width = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pallets", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pallets_Id_IsDeleted",
                table: "Pallets",
                columns: new[] { "Id", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Pallets_PackingListId_IsDeleted",
                table: "Pallets",
                columns: new[] { "PackingListId", "IsDeleted" });
        }
    }
}
