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
            new WaterTank { Id = Guid.Parse("2bf39e4b-0caa-4cda-8e28-883b88fce222"), Name = "Main water tank", Height = 180, Radius = 133, Volume = 10, CurrentWaterLevelId = Guid.Parse("74169af9-72b7-4313-971a-c96307b84fc9"), Statistics = null }
            );

            modelBuilder.Entity<WaterLevel>().HasData(
            new WaterLevel { Id = Guid.Parse("74169af9-72b7-4313-971a-c96307b84fc9"), DateTimeMeasured = DateTime.Now, Percentage = 90 }
            );

            modelBuilder.Entity<FireBeetle>().HasData(
            new FireBeetle { Id = Guid.Parse("e7379d81-1f29-494e-81e2-0a313541dd5e"), DateTimeMeasured = DateTime.Now, BatteryPercentage = 67 }
            );
        }
    }
}
