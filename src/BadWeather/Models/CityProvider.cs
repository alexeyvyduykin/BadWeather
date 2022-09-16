using BadWeather.Models;
using BadWeather.Services;
using BadWeather.Services.OpenWeather;
using BadWeather.ViewModels;

namespace BadWeather
{
    public class CityProvider
    {
        private DataService _dataService;
        private OpenWeatherService _openWeatherService;

        private List<CityViewModel>? _cache;

        public CityProvider(DataService dataService, OpenWeatherService openWeatherService)
        {
            _dataService = dataService;
            _openWeatherService = openWeatherService;
        }

        public async Task<IList<CityViewModel>> GetCitiesAsync()
        {
            if (_cache == null)
            {
                _cache = new List<CityViewModel>();

                var cities = await _dataService.GetDataAsync<City>("city");

                foreach (var item in cities.OrderBy(s => s.Name))
                {
                    var model = await _openWeatherService.GetModelAsync(item.Name!, item.Code!);

                    _cache.Add(new CityViewModel(model!));
                }
            }

            return _cache;
        }
    }
}
