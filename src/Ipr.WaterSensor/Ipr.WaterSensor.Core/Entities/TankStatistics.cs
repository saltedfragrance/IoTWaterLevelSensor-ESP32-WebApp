using Ipr.WaterSensor.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ipr.WaterSensor.Core.Entities
{
    public class TankStatistics : BaseEntity
    {
        public int TotalWaterCollected { get; set; }
        public int TotalWaterDispensed { get; set; }
    }
}
