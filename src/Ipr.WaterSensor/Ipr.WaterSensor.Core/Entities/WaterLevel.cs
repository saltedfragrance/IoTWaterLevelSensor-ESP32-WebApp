using Ipr.WaterSensor.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ipr.WaterSensor.Core.Entities
{
    public class WaterLevel : BaseEntity
    {
        public int WaterLevelInMeters { get; set; }
        public DateTime DateTimeMeasured { get; set; }
    }
}
