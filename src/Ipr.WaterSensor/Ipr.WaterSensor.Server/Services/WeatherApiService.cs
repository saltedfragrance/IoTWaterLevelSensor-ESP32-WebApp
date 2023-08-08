using Heron.MudCalendar;
using Ipr.WaterSensor.Core.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ipr.WaterSensor.Server.Services
{
    public class WeatherApiService
    {
        private static readonly HttpClient client = new HttpClient();
        //1mm = 1 liter water per m²
        public string ApiUrl { get; set; }
        public List<WeatherPrediction> WeatherData { get; set; }

        public WeatherApiService()
        {
            ApiUrl = "https://api.open-meteo.com/v1/forecast?latitude=51.1758&longitude=2.8758&daily=precipitation_sum,precipitation_probability_max&timezone=Europe%2FBerlin&forecast_days=16";
        }

        public async Task<List<CalendarItem>> GetData()
        {
            WeatherData = new();
            List<CalendarItem> calItems = new List<CalendarItem>();

            using HttpResponseMessage response = await client.GetAsync(ApiUrl);
            string responseBody = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(responseBody).Property("daily").Values();

            for (int i = 0; i < json.Children().ToList()[0].Count(); i++)
            {
                WeatherData.Add(new WeatherPrediction
                {
                    Date = Convert.ToDateTime(json.Children()[i].ToList()[0]),
                    Precipitation = json.Children()[i].ToList()[1].Value<decimal>(),
                    PrecipitationProbability = json.Children()[i].ToList()[2].Value<string>()
                });
            }

            foreach (var item in WeatherData)
            {
                var toAdd = new CalendarItem
                {
                    AllDay = true,
                    Start = Convert.ToDateTime(item.Date),
                };

                if (item.Precipitation > 0 && item.Precipitation < 5)
                {
                    toAdd.Text = "🌧";
                }

                if (item.Precipitation > 0 && item.Precipitation > 5)
                {
                    toAdd.Text = "⛈ ";
                }

                if(!string.IsNullOrEmpty(toAdd.Text)) calItems.Add(toAdd);
            }
            return calItems;
        }
    }
}
