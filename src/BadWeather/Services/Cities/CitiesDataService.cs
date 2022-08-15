using BadWeather.Models;
using Newtonsoft.Json;

namespace BadWeather.Services.Cities
{
    public class CitiesDataService
    {
        private readonly string _runningPath;
        private IList<City>? _cities;
        private IList<OpenWeatherCity>? _openWeatherCities;

        public CitiesDataService()
        {
            _runningPath = AppDomain.CurrentDomain.BaseDirectory;
        }

        public async Task<IList<City>> GetCitiesAsync()
        {
            if (_cities == null)
            {
                var path = string.Format("{0}resources\\cities60.json", Path.GetFullPath(Path.Combine(_runningPath, @"..\..\..\")));

                _cities = await Task.Run(() =>
                DeserializeFromStream<City>(path)
                .OrderByDescending(s => s.Population)
                .Take(10 /*60*/)
                .ToList());
            }

            return _cities;
        }

        public async Task<IList<OpenWeatherCity>> GetOpenWeatherCitiesAsync()
        {
            if (_openWeatherCities == null)
            {
                var path = string.Format("{0}resources\\openweatherCities60.json", Path.GetFullPath(Path.Combine(_runningPath, @"..\..\..\")));

                _openWeatherCities = await Task.Run(() =>
                DeserializeFromStream<OpenWeatherCity>(path)
                .ToList());
            }

            return _openWeatherCities;
        }

        private static List<T> DeserializeFromStream<T>(string path)
        {
            using StreamReader file = File.OpenText(path);
            var serializer = new JsonSerializer();
            return (List<T>)(serializer.Deserialize(file, typeof(List<T>)) ?? new List<T>());
        }
    }
}
