using Ipr.WaterSensor.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ipr.WaterSensor.Core.Entities
{
    public class AlarmEmail
    {
        public EmailTypes AlarmType { get; set; }
        public Person Recipient { get; set; }
    }
}
