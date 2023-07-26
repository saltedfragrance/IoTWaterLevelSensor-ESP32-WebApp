using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ipr.WaterSensor.Core.Entities
{
    public class WaterTank
    {
        public string Name { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int CubicMeters { get; set; }
        public int Liters { get; set; }
    }
}
