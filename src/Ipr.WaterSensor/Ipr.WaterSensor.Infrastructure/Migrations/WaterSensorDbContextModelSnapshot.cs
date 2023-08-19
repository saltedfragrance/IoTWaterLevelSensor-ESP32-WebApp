﻿// <auto-generated />
using System;
using Ipr.WaterSensor.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Ipr.WaterSensor.Infrastructure.Migrations
{
    [DbContext(typeof(WaterSensorDbContext))]
    partial class WaterSensorDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Ipr.WaterSensor.Core.Entities.AlarmEmail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AlarmType")
                        .HasColumnType("int");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("bit");

                    b.Property<Guid>("PersonId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("PersonId");

                    b.ToTable("AlarmEmails");

                    b.HasData(
                        new
                        {
                            Id = new Guid("8a53c07c-7114-4d23-b64e-9d9e9ca4b053"),
                            AlarmType = 0,
                            IsEnabled = false,
                            PersonId = new Guid("40d068a0-c84d-4171-a1fc-a637d324e8cc")
                        },
                        new
                        {
                            Id = new Guid("f3fc343c-71c7-4b2d-9c34-ee0db03a67be"),
                            AlarmType = 1,
                            IsEnabled = false,
                            PersonId = new Guid("40d068a0-c84d-4171-a1fc-a637d324e8cc")
                        });
                });

            modelBuilder.Entity("Ipr.WaterSensor.Core.Entities.FireBeetle", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("BatteryPercentage")
                        .HasColumnType("float");

                    b.Property<DateTime>("DateTimeMeasured")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("FireBeetleDevice");

                    b.HasData(
                        new
                        {
                            Id = new Guid("e7379d81-1f29-494e-81e2-0a313541dd5e"),
                            BatteryPercentage = 67.0,
                            DateTimeMeasured = new DateTime(2023, 8, 19, 16, 1, 1, 544, DateTimeKind.Local).AddTicks(2535)
                        });
                });

            modelBuilder.Entity("Ipr.WaterSensor.Core.Entities.Person", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("People");

                    b.HasData(
                        new
                        {
                            Id = new Guid("40d068a0-c84d-4171-a1fc-a637d324e8cc"),
                            EmailAddress = "stijn.vandekerckhove2@student.howest.be",
                            Name = "Stijn"
                        });
                });

            modelBuilder.Entity("Ipr.WaterSensor.Core.Entities.TankStatistics", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Month")
                        .HasColumnType("int");

                    b.Property<double>("TotalWaterConsumed")
                        .HasColumnType("float");

                    b.Property<Guid>("WaterTankId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("WaterTankId");

                    b.ToTable("TankStatistics");

                    b.HasData(
                        new
                        {
                            Id = new Guid("c236054c-b46c-4048-bd18-5338d842d2be"),
                            Month = 7,
                            TotalWaterConsumed = 200.0,
                            WaterTankId = new Guid("2bf39e4b-0caa-4cda-8e28-883b88fce222"),
                            Year = 2023
                        },
                        new
                        {
                            Id = new Guid("a45c57c2-4b4d-4a34-8536-14b04ebb3bdb"),
                            Month = 6,
                            TotalWaterConsumed = 300.0,
                            WaterTankId = new Guid("2bf39e4b-0caa-4cda-8e28-883b88fce222"),
                            Year = 2023
                        },
                        new
                        {
                            Id = new Guid("49d07b12-54be-4256-b25e-95d627bea39f"),
                            Month = 5,
                            TotalWaterConsumed = 500.0,
                            WaterTankId = new Guid("2bf39e4b-0caa-4cda-8e28-883b88fce222"),
                            Year = 2023
                        });
                });

            modelBuilder.Entity("Ipr.WaterSensor.Core.Entities.WaterLevel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateTimeMeasured")
                        .HasColumnType("datetime2");

                    b.Property<double>("Percentage")
                        .HasColumnType("float");

                    b.Property<Guid>("WaterTankId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("WaterTankId");

                    b.ToTable("WaterLevels");

                    b.HasData(
                        new
                        {
                            Id = new Guid("5c7d20cb-f950-41a1-8f1b-4e4259727d96"),
                            DateTimeMeasured = new DateTime(2023, 7, 12, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Percentage = 58.0,
                            WaterTankId = new Guid("2bf39e4b-0caa-4cda-8e28-883b88fce222")
                        },
                        new
                        {
                            Id = new Guid("7312af38-de1c-4f14-b621-7d95d7b94af1"),
                            DateTimeMeasured = new DateTime(2023, 7, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Percentage = 60.0,
                            WaterTankId = new Guid("2bf39e4b-0caa-4cda-8e28-883b88fce222")
                        },
                        new
                        {
                            Id = new Guid("b8704ae8-3dbf-4e96-b949-86900cd868b8"),
                            DateTimeMeasured = new DateTime(2023, 6, 12, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Percentage = 47.0,
                            WaterTankId = new Guid("2bf39e4b-0caa-4cda-8e28-883b88fce222")
                        },
                        new
                        {
                            Id = new Guid("3abafa70-015b-4946-94fb-887df2c4d268"),
                            DateTimeMeasured = new DateTime(2023, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Percentage = 50.0,
                            WaterTankId = new Guid("2bf39e4b-0caa-4cda-8e28-883b88fce222")
                        },
                        new
                        {
                            Id = new Guid("63985122-d59c-47d3-b509-ebbcbd9bf63c"),
                            DateTimeMeasured = new DateTime(2023, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Percentage = 55.0,
                            WaterTankId = new Guid("2bf39e4b-0caa-4cda-8e28-883b88fce222")
                        },
                        new
                        {
                            Id = new Guid("02b4c860-78af-491b-9e8c-ca1152485dbd"),
                            DateTimeMeasured = new DateTime(2023, 5, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Percentage = 60.0,
                            WaterTankId = new Guid("2bf39e4b-0caa-4cda-8e28-883b88fce222")
                        });
                });

            modelBuilder.Entity("Ipr.WaterSensor.Core.Entities.WaterTank", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("CurrentUpdateIntervalMinutes")
                        .HasColumnType("float");

                    b.Property<int>("Height")
                        .HasColumnType("int");

                    b.Property<bool>("IntervalChanged")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("NewUpdateIntervalMinutes")
                        .HasColumnType("float");

                    b.Property<int>("Radius")
                        .HasColumnType("int");

                    b.Property<int>("Volume")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("WaterTanks");

                    b.HasData(
                        new
                        {
                            Id = new Guid("2bf39e4b-0caa-4cda-8e28-883b88fce222"),
                            CurrentUpdateIntervalMinutes = 30.0,
                            Height = 180,
                            IntervalChanged = false,
                            Name = "Main water tank",
                            NewUpdateIntervalMinutes = 0.0,
                            Radius = 133,
                            Volume = 10
                        });
                });

            modelBuilder.Entity("Ipr.WaterSensor.Core.Entities.AlarmEmail", b =>
                {
                    b.HasOne("Ipr.WaterSensor.Core.Entities.Person", "Person")
                        .WithMany("SubscribedEmails")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");
                });

            modelBuilder.Entity("Ipr.WaterSensor.Core.Entities.TankStatistics", b =>
                {
                    b.HasOne("Ipr.WaterSensor.Core.Entities.WaterTank", "WaterTank")
                        .WithMany("TankStatistics")
                        .HasForeignKey("WaterTankId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WaterTank");
                });

            modelBuilder.Entity("Ipr.WaterSensor.Core.Entities.WaterLevel", b =>
                {
                    b.HasOne("Ipr.WaterSensor.Core.Entities.WaterTank", "WaterTank")
                        .WithMany("WaterLevels")
                        .HasForeignKey("WaterTankId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WaterTank");
                });

            modelBuilder.Entity("Ipr.WaterSensor.Core.Entities.Person", b =>
                {
                    b.Navigation("SubscribedEmails");
                });

            modelBuilder.Entity("Ipr.WaterSensor.Core.Entities.WaterTank", b =>
                {
                    b.Navigation("TankStatistics");

                    b.Navigation("WaterLevels");
                });
#pragma warning restore 612, 618
        }
    }
}
