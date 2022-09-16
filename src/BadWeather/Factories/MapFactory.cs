using BadWeather.Layers;
using BadWeather.Services;
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

        public Map CreateMap(string type)
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

            if (string.Equals(type, "world"))
            {
                map.Layers.Add(CreateWorldMapLayer());
            }
            else if (string.Equals(type, "bing"))
            {
                map.Layers.Add(CreateBingAerialMapLayer());
            }

            map.Layers.Add(layer);

            map.Home = n => n.NavigateTo(rect, Mapsui.Utilities.ScaleMethod.Fill);

            map.BackColor = Color.FromString("#000613");

            return map;
        }

        private static ILayer CreateWorldMapLayer()
        {
            var runningPath = AppDomain.CurrentDomain.BaseDirectory;

            var path = string.Format("{0}src\\BadWeather.Avalonia\\BadWeather.Avalonia\\Assets\\world.mbtiles", Path.GetFullPath(Path.Combine(runningPath, @"..\..\..\..\..\..\")));

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
            var dataService = _dependencyResolver.GetExistingService<DataService>();
            var openWeatherService = _dependencyResolver.GetExistingService<OpenWeatherService>();

            var layer = new CityLayer(dataService, openWeatherService)
            {
                Name = "Points with labels",
                IsMapInfoLayer = true,
                Style = new StyleCollection
                {
                    LayerStyles.CreatePointStyle(),
                    LayerStyles.CreateTemperatureLabel(),
                    LayerStyles.CreateCityLabel()
                }
            };

            return layer;
        }
    }
}
