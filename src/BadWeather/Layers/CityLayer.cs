using BadWeather.Models;
using BadWeather.Services;
using BadWeather.Services.OpenWeather;
using Mapsui;
using Mapsui.Extensions;
using Mapsui.Layers;
using Mapsui.Projections;
using ReactiveUI;
using System.Reactive.Linq;

namespace BadWeather.Layers
{
    public class CityLayer : MemoryLayer
    {
        public CityLayer(DataService dataService, OpenWeatherService openWeatherService)
        {
            var cities = dataService.GetData<City>("city")
                .OrderByDescending(s => s.Population)
                .Take(10 /*60*/)
                .ToList();

            var features = new List<IFeature>();

            foreach (var city in cities)
            {
                if (city != null
                    && city.Name is string name
                    && city.Code is string code
                    && city.Population is int population)
                {
                    var loading = ReactiveCommand.CreateFromTask(() => openWeatherService.GetModelAsync(name, code.ToUpper()));

                    loading.Execute()
                           .ObserveOn(RxApp.MainThreadScheduler)
                           .Where(s => s != null)
                           .Subscribe(s => features.Add(CreateFeature(s!, population)));
                }
            }

            Features = features;
        }

        private static IFeature CreateFeature(OpenWeatherModel model, int population)
        {
            var temp = model.Main?.GetTemperatureInCelsius() ?? default;

            var point = SphericalMercator.FromLonLat(model.Coordinate.Lon, model.Coordinate.Lat).ToMPoint();

            var feature = new PointFeature(point);

            feature["Name"] = model.Name;
            feature["Population"] = population;
            feature["Temperature"] = (int)Math.Round(temp);
            feature["Pressure"] = model.Main?.Pressure;
            feature["Humidity"] = model.Main?.Humidity;
            feature["Cloudiness"] = model.Clouds?.All;
            feature["Degree"] = model.Wind?.Degree;
            feature["Speed"] = model.Wind?.Speed;
            feature["Icon"] = model.Weathers?.First().Icon;

            return feature;
        }
    }
}
