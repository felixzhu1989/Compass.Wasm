using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Compass.QualityService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class qualityfinalinspector : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FinalInspectionCheckItemTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SequenceNumber = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinalInspectionCheckItemTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FinalInspections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InspectedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InspectedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Conclusion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinalInspections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FinalInspectionCheckItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FinalInspectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InspectorComments = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinalInspectionCheckItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinalInspectionCheckItems_FinalInspections_FinalInspectionId",
                        column: x => x.FinalInspectionId,
                        principalTable: "FinalInspections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FinalInspectionCheckItems_FinalInspectionId",
                table: "FinalInspectionCheckItems",
                column: "FinalInspectionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinalInspectionCheckItems");

            migrationBuilder.DropTable(
                name: "FinalInspectionCheckItemTypes");

            migrationBuilder.DropTable(
                name: "FinalInspections");
        }
    }
}
