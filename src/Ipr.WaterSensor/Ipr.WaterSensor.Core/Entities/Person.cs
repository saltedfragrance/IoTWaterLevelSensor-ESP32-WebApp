using Ipr.WaterSensor.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ipr.WaterSensor.Core.Entities
{
    public class Person : BaseEntity
    {
        public string Name { get; set; }
        public string EmailAddress { get; set; }
    }
}
