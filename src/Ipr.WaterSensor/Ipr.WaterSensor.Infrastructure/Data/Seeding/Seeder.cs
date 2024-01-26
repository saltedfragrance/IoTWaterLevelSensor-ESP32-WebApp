using Ipr.WaterSensor.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ipr.WaterSensor.Infrastructure.Data.Seeding
{
    public class Seeder
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WaterTank>().HasData(
            new WaterTank { Id = Guid.Parse("2bf39e4b-0caa-4cda-8e28-883b88fce222"), Name = "Main water tank", Height = 180, Radius = 133, Volume = 10, CurrentUpdateIntervalMinutes = 30, NewUpdateIntervalMinutes = 0, IntervalChanged = false }
            );

            modelBuilder.Entity<WaterLevel>().HasData(
            new WaterLevel { Id = Guid.Parse("5c7d20cb-f950-41a1-8f1b-4e4259727d96"), DateTimeMeasured = Convert.ToDateTime("2023/07/12"), Percentage = 58, WaterTankId = Guid.Parse("2bf39e4b-0caa-4cda-8e28-883b88fce222") },
            new WaterLevel { Id = Guid.Parse("7312af38-de1c-4f14-b621-7d95d7b94af1"), DateTimeMeasured = Convert.ToDateTime("2023/07/11"), Percentage = 60, WaterTankId = Guid.Parse("2bf39e4b-0caa-4cda-8e28-883b88fce222") },
            new WaterLevel { Id = Guid.Parse("b8704ae8-3dbf-4e96-b949-86900cd868b8"), DateTimeMeasured = Convert.ToDateTime("2023/06/12"), Percentage = 47, WaterTankId = Guid.Parse("2bf39e4b-0caa-4cda-8e28-883b88fce222") },
            new WaterLevel { Id = Guid.Parse("3abafa70-015b-4946-94fb-887df2c4d268"), DateTimeMeasured = Convert.ToDateTime("2023/06/11"), Percentage = 50, WaterTankId = Guid.Parse("2bf39e4b-0caa-4cda-8e28-883b88fce222") },
            new WaterLevel { Id = Guid.Parse("63985122-d59c-47d3-b509-ebbcbd9bf63c"), DateTimeMeasured = Convert.ToDateTime("2023/05/12"), Percentage = 55, WaterTankId = Guid.Parse("2bf39e4b-0caa-4cda-8e28-883b88fce222") },
            new WaterLevel { Id = Guid.Parse("02b4c860-78af-491b-9e8c-ca1152485dbd"), DateTimeMeasured = Convert.ToDateTime("2023/05/11"), Percentage = 60, WaterTankId = Guid.Parse("2bf39e4b-0caa-4cda-8e28-883b88fce222") }
            );

            modelBuilder.Entity<TankStatistics>().HasData(
            new TankStatistics { Id = Guid.NewGuid(), WaterTankId = Guid.Parse("2bf39e4b-0caa-4cda-8e28-883b88fce222"), Year = 2023, Month = 7, TotalWaterConsumed = 200 },
            new TankStatistics { Id = Guid.NewGuid(), WaterTankId = Guid.Parse("2bf39e4b-0caa-4cda-8e28-883b88fce222"), Year = 2023, Month = 6, TotalWaterConsumed = 300 },
            new TankStatistics { Id = Guid.NewGuid(), WaterTankId = Guid.Parse("2bf39e4b-0caa-4cda-8e28-883b88fce222"), Year = 2023, Month = 5, TotalWaterConsumed = 500 }
            );

            modelBuilder.Entity<FireBeetle>().HasData(
            new FireBeetle { Id = Guid.Parse("e7379d81-1f29-494e-81e2-0a313541dd5e"), DateTimeMeasured = DateTime.Now, BatteryPercentage = 67 }
            );

            modelBuilder.Entity<Person>().HasData(
            new Person
            {
                Id = Guid.Parse("40d068a0-c84d-4171-a1fc-a637d324e8cc"),
                Name = "Stijn",
                EmailAddress = "stijn.vandekerckhove2@student.howest.be"
            }
            );

            modelBuilder.Entity<AlarmEmail>().HasData(
            new AlarmEmail { Id = Guid.Parse("8a53c07c-7114-4d23-b64e-9d9e9ca4b053"), AlarmType = Core.Enums.EmailTypes.Batterij, IsEnabled = false, PersonId = Guid.Parse("40d068a0-c84d-4171-a1fc-a637d324e8cc") },
            new AlarmEmail { Id = Guid.Parse("f3fc343c-71c7-4b2d-9c34-ee0db03a67be"), AlarmType = Core.Enums.EmailTypes.Waterniveau, IsEnabled = false, PersonId = Guid.Parse("40d068a0-c84d-4171-a1fc-a637d324e8cc") }
            );
        }
    }
}
