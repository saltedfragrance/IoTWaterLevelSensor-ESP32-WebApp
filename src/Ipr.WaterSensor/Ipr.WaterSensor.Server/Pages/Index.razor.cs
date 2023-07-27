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

        private async Task GetTanksData()
        {
            using (WaterSensorDbContext context = DbContextFactory.CreateDbContext())
            {
                Tanks = await context.WaterTanks.Include(x => x.CurrentWaterLevel).ToListAsync();
            }
        }
    }
}
