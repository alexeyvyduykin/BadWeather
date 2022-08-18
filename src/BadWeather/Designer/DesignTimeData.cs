using BadWeather.Services.Cities;
using BadWeather.Services.OpenWeather;
using BadWeather.ViewModels;
using Mapsui;
using Mapsui.Layers;
using Splat;

namespace BadWeather.Designer
{
    public class DesignTimeData : IReadonlyDependencyResolver
    {
        private Map? _map;
        private MapFactory? _mapFactory;
        private ViewModelFactory? _viewModelFactory;
        private MainViewModel? _mainViewModel;
        private CitiesDataService? _citiesDataService;
        private OpenWeatherService? _openWeatherService;

        public object? GetService(Type? serviceType, string? contract = null)
        {
            if (serviceType == typeof(MapFactory))
            {
                return _mapFactory ??= new MapFactory(this);
            }
            else if (serviceType == typeof(ViewModelFactory))
            {
                return _viewModelFactory ??= new ViewModelFactory(this);
            }
            else if (serviceType == typeof(IMap))
            {
                return _map ??= CreateMap();
            }
            else if (serviceType == typeof(MainViewModel))
            {
                return _mainViewModel ??= new MainViewModel(this);
            }
            else if (serviceType == typeof(CitiesDataService))
            {
                return _citiesDataService ??= new CitiesDataService();
            }
            else if (serviceType == typeof(OpenWeatherService))
            {
                return _openWeatherService ??= new OpenWeatherService("fd8fe7226da08b68880c55c35f7ea4f8");
            }

            throw new Exception();
        }

        private static Map CreateMap()
        {
            var map = new Map();
            map.Layers.Add(new Layer() { Name = "WorldMap" });
            map.BackColor = Mapsui.Styles.Color.White;
            return map;
        }

        public static OpenWeatherModel BuildOpenWeatherModel()
        {
            return new OpenWeatherModel()
            {
                Name = "London",
                Coordinate = new Coordinate()
                {
                    Lon = -0.1257,
                    Lat = 51.5085
                },
                Weathers = new()
                {
                    new Weather()
                    {
                        Id = 803,
                        Main = "Clouds",
                        Description = "broken clouds",
                        Icon = "04d"
                    }
                },
                Base = "stations",
                Main = new Main()
                {
                    Temperature = 293.14,
                    TemperatureFeelsLike = 293.36,
                    MinTemperature = 291.96,
                    MaxTemperature = 294.69,
                    Pressure = 1014.0,
                    Humidity = 83
                },
                Visibility = 10000,
                Wind = new Wind()
                {
                    Speed = 0.51,
                    Degree = 0,
                    Gust = 0.0,
                },
                Clouds = new Clouds()
                {
                    All = 75,
                },
                Rain = null,
                Snow = null,
                DataTime = 1660812609,
                Sys = new Sys()
                {
                    Type = 2,
                    Id = 2075535,
                    Message = 0.0,
                    Country = "GB",
                    Sunrise = ToDateTime(1660798239),
                    Sunset = ToDateTime(1660850307)
                },
                Timezone = 3600,
                Id = 2643743,
                Cod = 200
            };
        }

        private static DateTime ToDateTime(int unixTime)
        {
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

            return dateTime.AddSeconds((double)unixTime).ToLocalTime();
        }

        public IEnumerable<object> GetServices(Type? serviceType, string? contract = null)
        {
            throw new Exception();
        }
    }
}
