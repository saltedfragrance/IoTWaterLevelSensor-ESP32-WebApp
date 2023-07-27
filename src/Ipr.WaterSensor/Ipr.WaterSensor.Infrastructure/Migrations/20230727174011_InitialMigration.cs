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
                    Percentage = table.Column<int>(type: "int", nullable: false),
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
                    WaterLevelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                        name: "FK_WaterTanks_WaterLevels_WaterLevelId",
                        column: x => x.WaterLevelId,
                        principalTable: "WaterLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "WaterLevels",
                columns: new[] { "Id", "DateTimeMeasured", "Percentage" },
                values: new object[,]
                {
                    { new Guid("74169af9-72b7-4313-971a-c96307b84fc9"), new DateTime(2023, 7, 27, 19, 40, 11, 639, DateTimeKind.Local).AddTicks(6039), 90 },
                    { new Guid("fe59d3ff-d8f5-43d4-8a0f-4a6e3976c8db"), new DateTime(2023, 7, 27, 19, 40, 11, 639, DateTimeKind.Local).AddTicks(6065), 45 }
                });

            migrationBuilder.InsertData(
                table: "WaterTanks",
                columns: new[] { "Id", "CubicMeters", "Height", "Liters", "Name", "StatisticsId", "WaterLevelId", "Width" },
                values: new object[,]
                {
                    { new Guid("2bf39e4b-0caa-4cda-8e28-883b88fce222"), 8, 4, 10000, "Main water tank", null, new Guid("74169af9-72b7-4313-971a-c96307b84fc9"), 2 },
                    { new Guid("457e0ed4-bf75-4d81-b2ac-063e0247bf58"), 8, 4, 10000, "Secondary water tank", null, new Guid("fe59d3ff-d8f5-43d4-8a0f-4a6e3976c8db"), 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_WaterTanks_StatisticsId",
                table: "WaterTanks",
                column: "StatisticsId");

            migrationBuilder.CreateIndex(
                name: "IX_WaterTanks_WaterLevelId",
                table: "WaterTanks",
                column: "WaterLevelId");
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
