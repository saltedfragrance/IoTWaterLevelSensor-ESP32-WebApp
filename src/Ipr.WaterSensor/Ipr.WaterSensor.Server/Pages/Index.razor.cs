using Ipr.WaterSensor.Server.Services;
using Microsoft.AspNetCore.Components;
using MQTTnet.Server;

namespace Ipr.WaterSensor.Server.Pages
{
    public partial class Index : ComponentBase
    {
        [Inject]
        public MQTTService MQTTService { get; set; } = default!;

        public const int waterTankHeight = 400;
        public int CurrentMeasurementCm { get; set; }
        public int CurrentWaterLevelCm { get; set; }
        private void CalculateWaterLevel()
        {
            CurrentWaterLevelCm =  waterTankHeight - CurrentMeasurementCm;
        }
    }
}
