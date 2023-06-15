using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Compass.PlanService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addpackinglist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PackingItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PackingListId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MtlNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<double>(type: "float", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Length = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Width = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Height = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Material = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CalcRule = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReCreate = table.Column<bool>(type: "bit", nullable: false),
                    Pallet = table.Column<bool>(type: "bit", nullable: false),
                    PrintLabel = table.Column<bool>(type: "bit", nullable: false),
                    OneLabel = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackingItems", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "PackingLists",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MainPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Product = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PackingType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeliveryType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackingLists", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "Pallets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PackingListId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PalletNumber = table.Column<int>(type: "int", nullable: false),
                    ItemNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemLength = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemWidth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemHeight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Length = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Width = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Height = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GrossWeight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NetWeight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pallets", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PackingItems_Id_IsDeleted",
                table: "PackingItems",
                columns: new[] { "Id", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_PackingItems_PackingListId_IsDeleted",
                table: "PackingItems",
                columns: new[] { "PackingListId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_PackingLists_Id_IsDeleted",
                table: "PackingLists",
                columns: new[] { "Id", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_PackingLists_MainPlanId_IsDeleted",
                table: "PackingLists",
                columns: new[] { "MainPlanId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Pallets_Id_IsDeleted",
                table: "Pallets",
                columns: new[] { "Id", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Pallets_PackingListId_IsDeleted",
                table: "Pallets",
                columns: new[] { "PackingListId", "IsDeleted" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PackingItems");

            migrationBuilder.DropTable(
                name: "PackingLists");

            migrationBuilder.DropTable(
                name: "Pallets");
        }
    }
}
