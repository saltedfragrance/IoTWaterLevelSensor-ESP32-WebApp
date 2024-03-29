﻿using System;
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
                name: "FireBeetleDevice",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BatteryPercentage = table.Column<double>(type: "float", nullable: false),
                    DateTimeMeasured = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FireBeetleDevice", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.Id);
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
                    CurrentUpdateIntervalMinutes = table.Column<double>(type: "float", nullable: false),
                    NewUpdateIntervalMinutes = table.Column<double>(type: "float", nullable: false),
                    IntervalChanged = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaterTanks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AlarmEmails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AlarmType = table.Column<int>(type: "int", nullable: false),
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlarmEmails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlarmEmails_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TankStatistics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WaterTankId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    TotalWaterConsumed = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TankStatistics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TankStatistics_WaterTanks_WaterTankId",
                        column: x => x.WaterTankId,
                        principalTable: "WaterTanks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WaterLevels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Percentage = table.Column<double>(type: "float", nullable: false),
                    DateTimeMeasured = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WaterTankId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaterLevels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WaterLevels_WaterTanks_WaterTankId",
                        column: x => x.WaterTankId,
                        principalTable: "WaterTanks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "FireBeetleDevice",
                columns: new[] { "Id", "BatteryPercentage", "DateTimeMeasured" },
                values: new object[] { new Guid("e7379d81-1f29-494e-81e2-0a313541dd5e"), 67.0, new DateTime(2023, 8, 19, 16, 1, 1, 544, DateTimeKind.Local).AddTicks(2535) });

            migrationBuilder.InsertData(
                table: "People",
                columns: new[] { "Id", "EmailAddress", "Name" },
                values: new object[] { new Guid("40d068a0-c84d-4171-a1fc-a637d324e8cc"), "stijn.vandekerckhove2@student.howest.be", "Stijn" });

            migrationBuilder.InsertData(
                table: "WaterTanks",
                columns: new[] { "Id", "CurrentUpdateIntervalMinutes", "Height", "IntervalChanged", "Name", "NewUpdateIntervalMinutes", "Radius", "Volume" },
                values: new object[] { new Guid("2bf39e4b-0caa-4cda-8e28-883b88fce222"), 30.0, 180, false, "Main water tank", 0.0, 133, 10 });

            migrationBuilder.InsertData(
                table: "AlarmEmails",
                columns: new[] { "Id", "AlarmType", "IsEnabled", "PersonId" },
                values: new object[,]
                {
                    { new Guid("8a53c07c-7114-4d23-b64e-9d9e9ca4b053"), 0, false, new Guid("40d068a0-c84d-4171-a1fc-a637d324e8cc") },
                    { new Guid("f3fc343c-71c7-4b2d-9c34-ee0db03a67be"), 1, false, new Guid("40d068a0-c84d-4171-a1fc-a637d324e8cc") }
                });

            migrationBuilder.InsertData(
                table: "TankStatistics",
                columns: new[] { "Id", "Month", "TotalWaterConsumed", "WaterTankId", "Year" },
                values: new object[,]
                {
                    { new Guid("49d07b12-54be-4256-b25e-95d627bea39f"), 5, 500.0, new Guid("2bf39e4b-0caa-4cda-8e28-883b88fce222"), 2023 },
                    { new Guid("a45c57c2-4b4d-4a34-8536-14b04ebb3bdb"), 6, 300.0, new Guid("2bf39e4b-0caa-4cda-8e28-883b88fce222"), 2023 },
                    { new Guid("c236054c-b46c-4048-bd18-5338d842d2be"), 7, 200.0, new Guid("2bf39e4b-0caa-4cda-8e28-883b88fce222"), 2023 }
                });

            migrationBuilder.InsertData(
                table: "WaterLevels",
                columns: new[] { "Id", "DateTimeMeasured", "Percentage", "WaterTankId" },
                values: new object[,]
                {
                    { new Guid("02b4c860-78af-491b-9e8c-ca1152485dbd"), new DateTime(2023, 5, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 60.0, new Guid("2bf39e4b-0caa-4cda-8e28-883b88fce222") },
                    { new Guid("3abafa70-015b-4946-94fb-887df2c4d268"), new DateTime(2023, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 50.0, new Guid("2bf39e4b-0caa-4cda-8e28-883b88fce222") },
                    { new Guid("5c7d20cb-f950-41a1-8f1b-4e4259727d96"), new DateTime(2023, 7, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 58.0, new Guid("2bf39e4b-0caa-4cda-8e28-883b88fce222") },
                    { new Guid("63985122-d59c-47d3-b509-ebbcbd9bf63c"), new DateTime(2023, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 55.0, new Guid("2bf39e4b-0caa-4cda-8e28-883b88fce222") },
                    { new Guid("7312af38-de1c-4f14-b621-7d95d7b94af1"), new DateTime(2023, 7, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 60.0, new Guid("2bf39e4b-0caa-4cda-8e28-883b88fce222") },
                    { new Guid("b8704ae8-3dbf-4e96-b949-86900cd868b8"), new DateTime(2023, 6, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 47.0, new Guid("2bf39e4b-0caa-4cda-8e28-883b88fce222") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlarmEmails_PersonId",
                table: "AlarmEmails",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_TankStatistics_WaterTankId",
                table: "TankStatistics",
                column: "WaterTankId");

            migrationBuilder.CreateIndex(
                name: "IX_WaterLevels_WaterTankId",
                table: "WaterLevels",
                column: "WaterTankId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlarmEmails");

            migrationBuilder.DropTable(
                name: "FireBeetleDevice");

            migrationBuilder.DropTable(
                name: "TankStatistics");

            migrationBuilder.DropTable(
                name: "WaterLevels");

            migrationBuilder.DropTable(
                name: "People");

            migrationBuilder.DropTable(
                name: "WaterTanks");
        }
    }
}
