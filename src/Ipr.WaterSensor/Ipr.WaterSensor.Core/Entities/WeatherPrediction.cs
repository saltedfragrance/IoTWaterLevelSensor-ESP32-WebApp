using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ipr.WaterSensor.Core.Entities
{
    public class WeatherPrediction
    {
        public DateTime Date { get; set; }
        public int Precipitation { get; set; }
        public int Rain { get; set; }
        public int Showers { get; set; }
        public int? PrecipitationProbability { get; set; }
    }
}
