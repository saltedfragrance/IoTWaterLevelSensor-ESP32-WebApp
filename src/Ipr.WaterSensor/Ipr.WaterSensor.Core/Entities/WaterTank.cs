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
        public Guid WaterLevelId { get; set; }
        public WaterLevel? CurrentWaterLevel { get; set; }
        public TankStatistics? Statistics { get; set; }
    }
}
