using Heron.MudCalendar;
using Ipr.WaterSensor.Core.Entities;
using Ipr.WaterSensor.Server.Services;
using Microsoft.AspNetCore.Components;

namespace Ipr.WaterSensor.Server.Pages
{
    public partial class Calendar : ComponentBase
    {
        [Inject]
        public WeatherApiService WeatherService { get; set; } = default!;
        public List<CalendarItem> RainyDays { get; set; }
        public WeatherPrediction SelectedDay { get; set; }
        public bool ToggleDetail { get; set; }

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
