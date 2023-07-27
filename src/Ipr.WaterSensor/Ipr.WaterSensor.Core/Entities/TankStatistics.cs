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
        //gemiddeld dagelijks gebruik
        //achterhalen adhv dagelijks gebruik en verwachtte neerslag hoe lang voorraad nog gaat meegaan
        public int TotalWaterCollected { get; set; }
        public int TotalWaterDispensed { get; set; }
    }
}
