using BadWeather.Services.Cities;
using BadWeather.Services.OpenWeather;
using BadWeather.ViewModels;

namespace BadWeather
{
    public class CityProvider
    {
        private CitiesDataService _citiesDataService;
        private OpenWeatherService _openWeatherService;

        private List<CityViewModel>? _cache;

        public CityProvider(CitiesDataService citiesDataService, OpenWeatherService openWeatherService)
        {
            _citiesDataService = citiesDataService;
            _openWeatherService = openWeatherService;
        }

        public async Task<IList<CityViewModel>> GetCitiesAsync()
        {
            if (_cache == null)
            {
                _cache = new List<CityViewModel>();

                var cities = await _citiesDataService.GetCitiesAsync();

                foreach (var item in cities)
                {
                    var model = await _openWeatherService.GetModelAsync(item.Name!, item.Code!);

                    _cache.Add(new CityViewModel(model!));
                }
            }

            return _cache;
        }
    }
}
