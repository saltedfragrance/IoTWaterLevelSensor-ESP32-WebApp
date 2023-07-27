using Ipr.WaterSensor.Core.Entities;
using Ipr.WaterSensor.Infrastructure.Data.Seeding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ipr.WaterSensor.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<WaterTank> WaterTanks { get; set; }
        public DbSet<TankStatistics> TankStatistics { get; set; }
        public DbSet<WaterLevel> WaterLevels { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
            base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Seeder.Seed(modelBuilder);
        }
    }
}
