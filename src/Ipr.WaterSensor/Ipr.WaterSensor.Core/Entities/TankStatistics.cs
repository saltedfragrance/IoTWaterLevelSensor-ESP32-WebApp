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
        public Guid WaterTankId { get; set; }
        public WaterTank WaterTank { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public double TotalWaterConsumed { get; set; }
    }
}
