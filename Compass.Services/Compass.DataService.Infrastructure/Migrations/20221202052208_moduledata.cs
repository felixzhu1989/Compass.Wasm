using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Compass.DataService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class moduledata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KvfData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModuleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Length = table.Column<double>(type: "float", nullable: false),
                    Width = table.Column<double>(type: "float", nullable: false),
                    Height = table.Column<double>(type: "float", nullable: false),
                    SidePanel = table.Column<int>(type: "int", nullable: false),
                    MiddleToRight = table.Column<double>(type: "float", nullable: false),
                    ExhaustSpigotLength = table.Column<double>(type: "float", nullable: false),
                    ExhaustSpigotWidth = table.Column<double>(type: "float", nullable: false),
                    ExhaustSpigotHeight = table.Column<double>(type: "float", nullable: false),
                    ExhaustSpigotNumber = table.Column<int>(type: "int", nullable: false),
                    SupplySpigotNumber = table.Column<double>(type: "float", nullable: false),
                    SupplySpigotDistance = table.Column<double>(type: "float", nullable: false),
                    LightType = table.Column<int>(type: "int", nullable: false),
                    SpotLightNumber = table.Column<int>(type: "int", nullable: false),
                    SpotLightDistance = table.Column<double>(type: "float", nullable: false),
                    LedLogo = table.Column<bool>(type: "bit", nullable: false),
                    DrainType = table.Column<int>(type: "int", nullable: false),
                    WaterCollection = table.Column<bool>(type: "bit", nullable: false),
                    BackToBack = table.Column<bool>(type: "bit", nullable: false),
                    BackCj = table.Column<bool>(type: "bit", nullable: false),
                    CoverBoard = table.Column<bool>(type: "bit", nullable: false),
                    Ansul = table.Column<bool>(type: "bit", nullable: false),
                    AnsulSide = table.Column<int>(type: "int", nullable: false),
                    AnsulDetector = table.Column<int>(type: "int", nullable: false),
                    AnsulDrop = table.Column<int>(type: "int", nullable: false),
                    Marvel = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KvfData", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "UviData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModuleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Length = table.Column<double>(type: "float", nullable: false),
                    Width = table.Column<double>(type: "float", nullable: false),
                    Height = table.Column<double>(type: "float", nullable: false),
                    SidePanel = table.Column<int>(type: "int", nullable: false),
                    MiddleToRight = table.Column<double>(type: "float", nullable: false),
                    ExhaustSpigotLength = table.Column<double>(type: "float", nullable: false),
                    ExhaustSpigotWidth = table.Column<double>(type: "float", nullable: false),
                    ExhaustSpigotHeight = table.Column<double>(type: "float", nullable: false),
                    ExhaustSpigotNumber = table.Column<int>(type: "int", nullable: false),
                    LightType = table.Column<int>(type: "int", nullable: false),
                    SpotLightNumber = table.Column<int>(type: "int", nullable: false),
                    SpotLightDistance = table.Column<double>(type: "float", nullable: false),
                    UvLightType = table.Column<int>(type: "int", nullable: false),
                    Bluetooth = table.Column<bool>(type: "bit", nullable: false),
                    LedLogo = table.Column<bool>(type: "bit", nullable: false),
                    DrainType = table.Column<int>(type: "int", nullable: false),
                    WaterCollection = table.Column<bool>(type: "bit", nullable: false),
                    BackToBack = table.Column<bool>(type: "bit", nullable: false),
                    BackCj = table.Column<bool>(type: "bit", nullable: false),
                    CoverBoard = table.Column<bool>(type: "bit", nullable: false),
                    Ansul = table.Column<bool>(type: "bit", nullable: false),
                    AnsulSide = table.Column<int>(type: "int", nullable: false),
                    AnsulDetector = table.Column<int>(type: "int", nullable: false),
                    AnsulDrop = table.Column<int>(type: "int", nullable: false),
                    Marvel = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UviData", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KvfData");

            migrationBuilder.DropTable(
                name: "UviData");
        }
    }
}
