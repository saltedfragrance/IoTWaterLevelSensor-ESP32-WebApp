using Ipr.WaterSensor.Core.Entities;
using Ipr.WaterSensor.Infrastructure.Data;
using Ipr.WaterSensor.Server.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System;

namespace Ipr.WaterSensor.Server.Pages
{
    public partial class Index : ComponentBase
    {
        [Inject]
        public MQTTService MQTTService { get; set; } = default!;
        [Inject]
        protected IDbContextFactory<WaterSensorDbContext> DbContextFactory { get; set; } = default!;
        public List<WaterTank> Tanks { get; set; } = default!;
        public FireBeetle FireBeetleDevice { get; set; } = default!;
        private async Task GetData()
        {
            using (WaterSensorDbContext context = DbContextFactory.CreateDbContext())
            {
                Tanks = await context.WaterTanks.Include(x => x.CurrentWaterLevel).ToListAsync();
                FireBeetleDevice = await context.FireBeetleDevice.FirstOrDefaultAsync();
            }
        }

        private string GetWaterLevelPixels(int percentage)
        {
            var pixels = (210 - (percentage * 2) - 35).ToString() + "px";
            return pixels;
        }

        private void UpdateWaterTankLevel(string newLevel)
        {

        }
    }
}
