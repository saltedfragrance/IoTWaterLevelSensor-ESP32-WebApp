using Heron.MudCalendar;
using Ipr.WaterSensor.Core.Entities;
using Ipr.WaterSensor.Infrastructure.Data;
using Ipr.WaterSensor.Server.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Ipr.WaterSensor.Server.Pages
{
    public partial class Calendar : ComponentBase
    {
        [Inject]
        protected IDbContextFactory<WaterSensorDbContext> DbContextFactory { get; set; } = default!;
        [Inject]
        public WeatherApiService WeatherService { get; set; } = default!;
        public List<WaterTank> Tanks { get; set; } = default!;
        public WaterLevel LastWaterLevel { get; set; } = default!;
        public Guid CurrentSelectedTankId { get; set; }
        public List<CalendarItem> RainyDays { get; set; }
        public WeatherPrediction SelectedDay { get; set; }
        public bool ToggleDetail { get; set; }
        private async Task GetTanksData()
        {
            using (WaterSensorDbContext context = DbContextFactory.CreateDbContext())
            {
                Tanks = await context.WaterTanks.ToListAsync();
                if (CurrentSelectedTankId == Guid.Empty)
                {
                    CurrentSelectedTankId = Tanks.FirstOrDefault().Id;
                }
                LastWaterLevel = await context.WaterLevels.OrderByDescending(level => level.DateTimeMeasured).FirstOrDefaultAsync();
            }

        }
        public void ToggleRainyDayDetail(DateTime date)
        {
            ToggleDetail = false;
            SelectedDay = null;
            SelectedDay = WeatherService.WeatherData.FirstOrDefault(data => data.Date == date);

            if (SelectedDay != null && RainyDays.Any(day => day.Start == SelectedDay.Date))
            {
                ToggleDetail = true;
            }
        }
    }
}
