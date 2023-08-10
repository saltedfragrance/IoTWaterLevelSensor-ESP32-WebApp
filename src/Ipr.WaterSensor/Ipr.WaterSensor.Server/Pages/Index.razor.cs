using Ipr.WaterSensor.Core.Entities;
using Ipr.WaterSensor.Infrastructure.Data;
using Ipr.WaterSensor.Server.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using MQTTnet.Client;
using System;
using System.Globalization;
using System.Text;

namespace Ipr.WaterSensor.Server.Pages
{
    public partial class Index : ComponentBase
    {
        [Inject]
        public MQTTService MQTTService { get; set; } = default!;
        [Inject]
        protected IDbContextFactory<WaterSensorDbContext> DbContextFactory { get; set; } = default!;
        public WaterTank Tank { get; set; } = default!;
        public double CurrentWaterLevelPercentage { get; set; }
        public FireBeetle FireBeetleDevice { get; set; } = default!;
        private async Task GetData()
        {
            using (WaterSensorDbContext context = DbContextFactory.CreateDbContext())
            {
                Tank = await context.WaterTanks.Include(x => x.CurrentWaterLevel).FirstOrDefaultAsync();
                FireBeetleDevice = await context.FireBeetleDevice.FirstOrDefaultAsync();
            }
            CurrentWaterLevelPercentage = Math.Round(Tank.CurrentWaterLevel.Percentage, 2);
        }

        private string GetWaterLevelPixels(double percentage)
        {
            var pixels = (210 - (percentage * 2) - 35).ToString() + "px";
            return pixels;
        }

        private void UpdateWaterTankLevel(string measuredValue)
        {
            var newVolume = Tank.Radius * ((Tank.Height + 60) - Convert.ToDouble(measuredValue));
            using (WaterSensorDbContext context = DbContextFactory.CreateDbContext())
            {
                var newPercentage = (newVolume / Tank.Volume) * 100;

                if (newPercentage > 100) newPercentage = 100;
                else if (newPercentage < 0) newPercentage = 0;

                var waterLevel = new WaterLevel { DateTimeMeasured = DateTime.Now, Id = Guid.NewGuid(), Percentage = newPercentage };
                var tankToUpdate = context.WaterTanks.First(x => x.Id == Tank.Id);
                tankToUpdate.CurrentWaterLevelId = waterLevel.Id;
                context.Add(waterLevel);
                context.Update(tankToUpdate);
                context.SaveChanges();
            }
        }

        private async Task UpdateBatteryLevel(string measuredValue)
        {
            using (WaterSensorDbContext context = DbContextFactory.CreateDbContext())
            {
                context.FireBeetleDevice.First().BatteryPercentage = Convert.ToDouble(measuredValue);
                await context.SaveChangesAsync();
            }
        }
    }
}
