using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Compass.DataService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class newmodeldata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ModuleData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModuleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Length = table.Column<double>(type: "float", nullable: false),
                    Width = table.Column<double>(type: "float", nullable: false),
                    Height = table.Column<double>(type: "float", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SidePanel = table.Column<int>(type: "int", nullable: true),
                    MiddleToRight = table.Column<double>(type: "float", nullable: true),
                    ExhaustSpigotLength = table.Column<double>(type: "float", nullable: true),
                    ExhaustSpigotWidth = table.Column<double>(type: "float", nullable: true),
                    ExhaustSpigotHeight = table.Column<double>(type: "float", nullable: true),
                    ExhaustSpigotNumber = table.Column<int>(type: "int", nullable: true),
                    SupplySpigotNumber = table.Column<double>(type: "float", nullable: true),
                    SupplySpigotDistance = table.Column<double>(type: "float", nullable: true),
                    LightType = table.Column<int>(type: "int", nullable: true),
                    SpotLightNumber = table.Column<int>(type: "int", nullable: true),
                    SpotLightDistance = table.Column<double>(type: "float", nullable: true),
                    LedLogo = table.Column<bool>(type: "bit", nullable: true),
                    DrainType = table.Column<int>(type: "int", nullable: true),
                    WaterCollection = table.Column<bool>(type: "bit", nullable: true),
                    BackToBack = table.Column<bool>(type: "bit", nullable: true),
                    BackCj = table.Column<bool>(type: "bit", nullable: true),
                    CoverBoard = table.Column<bool>(type: "bit", nullable: true),
                    Ansul = table.Column<bool>(type: "bit", nullable: true),
                    AnsulSide = table.Column<int>(type: "int", nullable: true),
                    AnsulDetector = table.Column<int>(type: "int", nullable: true),
                    AnsulDrop = table.Column<int>(type: "int", nullable: true),
                    Marvel = table.Column<bool>(type: "bit", nullable: true),
                    UvLightType = table.Column<int>(type: "int", nullable: true),
                    Bluetooth = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleData", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ModuleData");
        }
    }
}
