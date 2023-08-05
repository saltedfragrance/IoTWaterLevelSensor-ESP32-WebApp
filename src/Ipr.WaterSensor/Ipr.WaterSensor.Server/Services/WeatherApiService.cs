using Heron.MudCalendar;
using Ipr.WaterSensor.Core.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ipr.WaterSensor.Server.Services
{
    public class WeatherApiService
    {
        static readonly HttpClient client = new HttpClient();
        //1mm = 1 liter water per m²
        public string ApiUrl { get; set; }

        public WeatherApiService()
        {
            ApiUrl = "https://api.open-meteo.com/v1/forecast?latitude=51.1758&longitude=2.8758&daily=precipitation_sum,rain_sum,showers_sum,precipitation_probability_max&timezone=Europe%2FBerlin&forecast_days=16";
        }

        public async Task<List<CalendarItem>> GetData()
        {
            List<WeatherPrediction> data = new List<WeatherPrediction>();

            using HttpResponseMessage response = await client.GetAsync(ApiUrl);
            string responseBody = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(responseBody).Property("daily").Values();

            var bla = json.Children().ToList();
            var count = json.Children()[0].Children();

            for (int i = 0; i < json.Children().ToList()[0].Count(); i++)
            {
                data.Add(new WeatherPrediction
                {
                    Date = Convert.ToDateTime(json.Children()[i].ToList()[0]),
                    Precipitation = json.Children()[i].ToList()[1].Value<string>(),
                    Rain = json.Children()[i].ToList()[2].Value<string>(),
                    Showers = json.Children()[i].ToList()[3].Value<string>(),
                    PrecipitationProbability = json.Children()[i].ToList()[4].Value<string>()
                });

            }
            return null;
        }
    }
}
