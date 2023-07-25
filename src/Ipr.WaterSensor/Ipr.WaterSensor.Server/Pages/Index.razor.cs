using Microsoft.AspNetCore.Components;

namespace Ipr.WaterSensor.Server.Pages
{
    public partial class Index : ComponentBase
    {
        public const int waterTankHeight = 400;
        public int CurrentMeasurementCm { get; set; }
        public int CurrentWaterLevelCm { get; set; }

        public Index()
        {
            CurrentWaterLevelCm = CalculateWaterLevel();
        }

        private int CalculateWaterLevel()
        {
            return waterTankHeight - CurrentMeasurementCm;
        }
    }
}
