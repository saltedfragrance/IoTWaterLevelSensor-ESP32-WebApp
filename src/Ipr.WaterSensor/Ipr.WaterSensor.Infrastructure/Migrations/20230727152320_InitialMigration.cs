using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ipr.WaterSensor.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TankStatistics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalWaterCollected = table.Column<int>(type: "int", nullable: false),
                    TotalWaterDispensed = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TankStatistics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WaterLevels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WaterLevelInMeters = table.Column<int>(type: "int", nullable: false),
                    DateTimeMeasured = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaterLevels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WaterTanks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    Width = table.Column<int>(type: "int", nullable: false),
                    CubicMeters = table.Column<int>(type: "int", nullable: false),
                    Liters = table.Column<int>(type: "int", nullable: false),
                    CurrentWaterLevelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StatisticsId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaterTanks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WaterTanks_TankStatistics_StatisticsId",
                        column: x => x.StatisticsId,
                        principalTable: "TankStatistics",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WaterTanks_WaterLevels_CurrentWaterLevelId",
                        column: x => x.CurrentWaterLevelId,
                        principalTable: "WaterLevels",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "WaterTanks",
                columns: new[] { "Id", "CubicMeters", "CurrentWaterLevelId", "Height", "Liters", "Name", "StatisticsId", "Width" },
                values: new object[,]
                {
                    { new Guid("62dc7ae0-c7f7-4640-886a-b144fe102e3f"), 8, null, 4, 10000, "Main water tank", null, 2 },
                    { new Guid("ae7058dc-a979-4de1-b97a-cfb20190dce9"), 8, null, 4, 10000, "Secondary water tank", null, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_WaterTanks_CurrentWaterLevelId",
                table: "WaterTanks",
                column: "CurrentWaterLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_WaterTanks_StatisticsId",
                table: "WaterTanks",
                column: "StatisticsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WaterTanks");

            migrationBuilder.DropTable(
                name: "TankStatistics");

            migrationBuilder.DropTable(
                name: "WaterLevels");
        }
    }
}
