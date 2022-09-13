using BadWeather.Services.Cities;
using BadWeather.Services.OpenWeather;
using BadWeather.Styles;
using BruTile;
using BruTile.Cache;
using BruTile.MbTiles;
using BruTile.Predefined;
using BruTile.Web;
using Mapsui;
using Mapsui.Extensions;
using Mapsui.Layers;
using Mapsui.Projections;
using Mapsui.Styles;
using Mapsui.Tiling.Fetcher;
using Mapsui.Tiling.Layers;
using Mapsui.UI;
using Splat;
using SQLite;

namespace BadWeather
{
    public static class BingArial
    {
        public static IPersistentCache<byte[]>? DefaultCache = null;
    }

    public class MapFactory
    {
        private readonly IReadonlyDependencyResolver _dependencyResolver;

        public MapFactory(IReadonlyDependencyResolver dependencyResolver)
        {
            _dependencyResolver = dependencyResolver;
        }

        public Map CreateMap()
        {
            var lon0 = -15.0;
            var lat0 = 59.5;
            var lon1 = 5.0;
            var lat1 = 49.5;

            //  (-12, 61)
            //  (5, 49)

            var point0 = SphericalMercator.FromLonLat(lon0, lat0).ToMPoint();
            var point1 = SphericalMercator.FromLonLat(lon1, lat1).ToMPoint();
            var center = new MPoint(point0.X + (point1.X - point0.X) / 2.0, point1.Y + (point0.Y - point1.Y) / 2.0);
            var rect = new MRect(point0.X, point0.Y, point1.X, point1.Y);

            var map = new Map()
            {
                CRS = "EPSG:3857",
                Limiter = new ViewportLimiterKeepWithin()
                {
                    PanLimits = rect,
                },
                //Limiter = new ViewportLimiterKeepWithin(),
                //Limiter = new ViewportLimiterWithoutLimits(),   
            };

            var layer = CreateLayer();

            map.Layers.Add(CreateWorldMapLayer());
            //map.Layers.Add(CreateBingAerialMapLayer());                                 
            map.Layers.Add(layer);

            map.Home = n => n.NavigateTo(rect, Mapsui.Utilities.ScaleMethod.Fill);

            map.BackColor = Color.FromString("#000613");

            return map;
        }

        private static TileLayer CreateWorldMapLayer()
        {
            var runningPath = AppDomain.CurrentDomain.BaseDirectory;

            var path = string.Format("{0}resources\\world.mbtiles", Path.GetFullPath(Path.Combine(runningPath, @"..\..\..\")));

            var mbTilesTileSource = new MbTilesTileSource(new SQLiteConnectionString(path, true));

            return new TileLayer(mbTilesTileSource) { Name = "WorldMap" };
        }

        private static TileLayer CreateBingAerialMapLayer()
        {
            IPersistentCache<byte[]>? persistentCache = BingArial.DefaultCache;

            string? apiKey = "AnmVEcFBluLkadprF4M4x5B4WrSRUhdrUhPEuYcAkDtjQSGBbAfeJCCWNeeSTcIx";

            var tileSource = new HttpTileSource(
                new GlobalSphericalMercator(Math.Max(1, 0), Math.Min(19, 20)),
                "https://t{s}.tiles.virtualearth.net/tiles/a{quadkey}.jpeg?g=517&token={k}",
                new[] { "0", "1", "2", "3", "4", "5", "6", "7" },
                apiKey,
                "BingAerial",
                persistentCache,
                tileFetcher: null,
                new Attribution("© Microsoft"),
                userAgent: null);

            // DataFetchStrategy prefetches tiles from higher levels
            return new TileLayer(tileSource, dataFetchStrategy: new DataFetchStrategy())
            {
                Name = "Bing Aerial",
            };
        }

        public ILayer CreateLayer()
        {
            var citiesDataService = _dependencyResolver.GetExistingService<CitiesDataService>();
            var openWeatherService = _dependencyResolver.GetExistingService<OpenWeatherService>();
            var cities = citiesDataService.GetCitiesAsync().Result;

            var features = cities.Select(city =>
            {
                var name = city.Name ?? throw new Exception();
                var code = city.Code?.ToUpper() ?? throw new Exception();
                var population = city.Population ?? throw new Exception();

                var model = Task.Run(async () => await openWeatherService.GetModelAsync(name, code)).Result;

                if (model == null)
                {
                    throw new Exception();
                }

                var feature = CreateFeature(model, population);

                return feature;
            });

            var layer = new MemoryLayer
            {
                Name = "Points with labels",
                Features = features.ToList(),
                //Style = LayerStyles.CreatePointStyle(),
                IsMapInfoLayer = true,
            };

            var coll = new StyleCollection();

            coll.Add(LayerStyles.CreatePointStyle());
            coll.Add(LayerStyles.CreateTemperatureLabel());
            coll.Add(LayerStyles.CreateCityLabel());

            layer.Style = coll;

            return layer;
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

            return feature;
        }
    }
}
