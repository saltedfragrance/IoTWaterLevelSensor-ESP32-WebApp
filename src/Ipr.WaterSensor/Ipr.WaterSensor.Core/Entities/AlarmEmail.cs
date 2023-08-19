using Ipr.WaterSensor.Core.Entities.Base;
using Ipr.WaterSensor.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ipr.WaterSensor.Core.Entities
{
    public class AlarmEmail : BaseEntity
    {
        public bool IsEnabled { get; set; }
        public EmailTypes AlarmType { get; set; }
        public Guid PersonId { get; set; }
        public Person Person { get; set; }
    }
}
