using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ipr.WaterSensor.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FireBeetleDevice",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BatteryPercentage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateTimeMeasured = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FireBeetleDevice", x => x.Id);
                });

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
                    Radius = table.Column<int>(type: "int", nullable: false),
                    Volume = table.Column<int>(type: "int", nullable: false),
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
                table: "FireBeetleDevice",
                columns: new[] { "Id", "BatteryPercentage", "DateTimeMeasured" },
                values: new object[] { new Guid("e7379d81-1f29-494e-81e2-0a313541dd5e"), "67", new DateTime(2023, 8, 10, 10, 51, 52, 210, DateTimeKind.Local).AddTicks(6707) });

            migrationBuilder.InsertData(
                table: "WaterLevels",
                columns: new[] { "Id", "DateTimeMeasured", "Percentage" },
                values: new object[] { new Guid("74169af9-72b7-4313-971a-c96307b84fc9"), new DateTime(2023, 8, 10, 10, 51, 52, 210, DateTimeKind.Local).AddTicks(6667), 90 });

            migrationBuilder.InsertData(
                table: "WaterTanks",
                columns: new[] { "Id", "Height", "Name", "Radius", "StatisticsId", "Volume", "WaterLevelId" },
                values: new object[] { new Guid("2bf39e4b-0caa-4cda-8e28-883b88fce222"), 180, "Main water tank", 133, null, 10, new Guid("74169af9-72b7-4313-971a-c96307b84fc9") });

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
                name: "FireBeetleDevice");

            migrationBuilder.DropTable(
                name: "WaterTanks");

            migrationBuilder.DropTable(
                name: "TankStatistics");

            migrationBuilder.DropTable(
                name: "WaterLevels");
        }
    }
}
