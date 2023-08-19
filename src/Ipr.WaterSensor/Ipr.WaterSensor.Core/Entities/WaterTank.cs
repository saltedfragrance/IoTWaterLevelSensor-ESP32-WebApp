using Ipr.WaterSensor.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ipr.WaterSensor.Core.Entities
{
    public class WaterTank : BaseEntity
    {
        public string Name { get; set; }
        public int Height { get; set; }
        public int Radius { get; set; }
        public int Volume { get; set; }
        public double CurrentUpdateIntervalMinutes { get; set; }
        public double NewUpdateIntervalMinutes { get; set; }
        public bool IntervalChanged { get; set; }
        public ICollection<WaterLevel> WaterLevels { get; set; }
        public ICollection<TankStatistics> TankStatistics { get; set; }
}
}
