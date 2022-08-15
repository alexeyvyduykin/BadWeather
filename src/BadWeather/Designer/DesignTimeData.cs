using BadWeather.Services.Cities;
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

            throw new Exception();
        }

        private static Map CreateMap()
        {
            var map = new Map();
            map.Layers.Add(new Layer() { Name = "WorldMap" });
            map.BackColor = Mapsui.Styles.Color.Red;
            return map;
        }

        public IEnumerable<object> GetServices(Type? serviceType, string? contract = null)
        {
            throw new Exception();
        }
    }
}
