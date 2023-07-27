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
            new WaterTank { Id = Guid.NewGuid(), Name = "Main water tank", Height = 4, Width = 2, CubicMeters = 8, Liters = 10000, CurrentWaterLevel = null, Statistics = null },
            new WaterTank { Id = Guid.NewGuid(), Name = "Secondary water tank", Height = 4, Width = 2, CubicMeters = 8, Liters = 10000, CurrentWaterLevel = null, Statistics = null }
            );
        }
    }
}
