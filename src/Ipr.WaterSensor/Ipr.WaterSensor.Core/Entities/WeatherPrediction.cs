﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ipr.WaterSensor.Core.Entities
{
    public class WeatherPrediction
    {
        public DateTime Date { get; set; }
        public decimal? Precipitation { get; set; }
        public string PrecipitationProbability { get; set; }
    }
}
