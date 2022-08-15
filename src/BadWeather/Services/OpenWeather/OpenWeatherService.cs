using Newtonsoft.Json;

namespace BadWeather.Services.OpenWeather
{
    public class OpenWeatherService
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        public OpenWeatherService(string apiKey)
        {
            _apiKey = apiKey;
            _httpClient = new HttpClient();
        }

        private Uri GenerateRequestUrl(double lon, double lat) =>
            new($"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={_apiKey}");

        private Uri GenerateRequestUrl(IList<double> ids)
        {
            string res = string.Empty;
            for (int i = 0; i < ids.Count; i++)
            {
                res += $"{ids[i]}";

                if (i != ids.Count - 1)
                {
                    res += $",";
                }
            }

            return new($"https://api.openweathermap.org/data/2.5/weather?id={res}&appid={_apiKey}");
        }

        private Uri GenerateRequestUrl(double id) => new($"https://api.openweathermap.org/data/2.5/weather?id={id}&appid={_apiKey}");


        private Uri GenerateRequestUrl(string name, string code) => 
            new($"https://api.openweathermap.org/data/2.5/weather?q={name},{code}&appid={_apiKey}");

        public async Task<OpenWeatherModel?> GetModelAsync(double lon, double lat)
        {
            var url = GenerateRequestUrl(lon, lat);
            var jsonString = await _httpClient.GetStringAsync(url).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<OpenWeatherModel>(jsonString);
        }

        public async Task<List<OpenWeatherModel>?> GetModelsAsync(IList<double> ids)
        {
            var url = GenerateRequestUrl(ids);
            var jsonString = await _httpClient.GetStringAsync(url).ConfigureAwait(false);
            return new List<OpenWeatherModel>() { JsonConvert.DeserializeObject<OpenWeatherModel>(jsonString) };
        }

        public async Task<OpenWeatherModel?> GetModelAsync(double id)
        {
            var url = GenerateRequestUrl(id);
            var jsonString = await _httpClient.GetStringAsync(url).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<OpenWeatherModel>(jsonString);
        }

        public async Task<OpenWeatherModel?> GetModelAsync(string name, string code)
        {
            var url = GenerateRequestUrl(name, code);
            var jsonString = await _httpClient.GetStringAsync(url).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<OpenWeatherModel>(jsonString);
        }
    }
}
