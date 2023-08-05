using Ipr.WaterSensor.Server.Services;
using Microsoft.AspNetCore.Components;

namespace Ipr.WaterSensor.Server.Pages
{
    public partial class Calendar : ComponentBase
    {
        [Inject]
        public WeatherApiService WeatherService { get; set; } = default!;
    }
}
