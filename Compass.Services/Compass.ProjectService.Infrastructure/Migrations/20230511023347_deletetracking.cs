using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Compass.ProjectService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class deletetracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Trackings");

            migrationBuilder.AddColumn<Guid>(
                name: "Designer",
                table: "Projects",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Designer",
                table: "Projects");

            migrationBuilder.CreateTable(
                name: "Trackings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProblemNotResolved = table.Column<bool>(type: "bit", nullable: false),
                    ShippingEndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ShippingStartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SortDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WarehousingTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trackings", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Trackings_Id_IsDeleted",
                table: "Trackings",
                columns: new[] { "Id", "IsDeleted" });
        }
    }
}
