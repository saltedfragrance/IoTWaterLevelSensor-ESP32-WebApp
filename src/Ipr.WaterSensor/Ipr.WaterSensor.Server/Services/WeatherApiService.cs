using Heron.MudCalendar;

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

        public async Task GetData()
        {
            using HttpResponseMessage response = await client.GetAsync(ApiUrl);
            string responseBody = await response.Content.ReadAsStringAsync();
        }
    }
}
