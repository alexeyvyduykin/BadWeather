using BadWeather.Layers;
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
        private static readonly OpenWeatherService _openWeatherService;

        static MapFactory()
        {
            _openWeatherService = new OpenWeatherService("fd8fe7226da08b68880c55c35f7ea4f8");
        }

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

            map.Info += MapOnInfo;
            var layer = CreateLayer();
            var layer2 = CreateLayer2(layer);
            var layer3 = CreateLayer3(layer);

            map.Layers.Add(CreateWorldMapLayer());
            //map.Layers.Add(CreateBingAerialMapLayer());                        
            map.Layers.Add(layer);
            map.Layers.Add(layer2);
            map.Layers.Add(layer3);

            map.Home = n => n.NavigateTo(rect, Mapsui.Utilities.ScaleMethod.Fill);

            map.BackColor = Color.FromString("#000613");

            return map;
        }

        private static void MapOnInfo(object sender, MapInfoEventArgs e)
        {
            var calloutStyle = e.MapInfo?.Feature?.Styles.Where(s => s is CalloutStyle).Cast<CalloutStyle>().FirstOrDefault();
            if (calloutStyle != null)
            {
                calloutStyle.Enabled = !calloutStyle.Enabled;
                e.MapInfo?.Layer?.DataHasChanged(); // To trigger a refresh of graphics.
            }
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

        //private static MemoryLayer CreatePointLayer()
        //{
        //    return new MemoryLayer
        //    {
        //        Name = "Points",
        //        IsMapInfoLayer = true,
        //        Features = GetCitiesFromEmbeddedResource(),
        //        Style = LayerStyles.CreateBitmapStyle()
        //    };
        //}

        //private static IEnumerable<IFeature> GetCitiesFromEmbeddedResource()
        //{
        //    return _cities.Select(c =>
        //    {
        //        var feature = new PointFeature(SphericalMercator.FromLonLat(c.Lon, c.Lat).ToMPoint());
        //        feature["name"] = c.Name;
        //        feature["country"] = c.Country;
        //        return feature;
        //    });
        //}

        public ILayer CreateLayer()
        {
            var citiesDataService = _dependencyResolver.GetExistingService<CitiesDataService>();

            var cities = citiesDataService.GetCitiesAsync().Result;

            var features = cities.Select(city =>
            {
                var name = city?.Name ?? default;
                var code = city?.Code?.ToUpper() ?? default;
                var population = city?.Population ?? default;

                var model = Task.Run(async () => await _openWeatherService.GetModelAsync(name, code)).Result;

                var feature = CreateFeature(model, population);

                return feature;
            });

            var layer = new MemoryLayer
            {
                Name = "Points with labels",
                Features = features.ToList(),
                //Style = null,
                Style = LayerStyles.CreateLabel_Style(),// null,
                IsMapInfoLayer = true,
            };

            return layer;
        }

        public ILayer CreateLayer2(ILayer layer)
        {
            return new VertexOnlyLayer(layer)
            {
                Name = "Points with labels",
                //Style = null,
                Style = LayerStyles.CreateTopLabelStyle(),
                IsMapInfoLayer = true,
            };
        }
        public ILayer CreateLayer3(ILayer layer)
        {
            return new VertexOnlyLayer(layer)
            {
                Name = "Points with labels",
                //Style = null,
                Style = LayerStyles.CreateBottomLabelStyle(),
                IsMapInfoLayer = true,
            };
        }
        private static IFeature CreateFeature(OpenWeatherModel city, int population)
        {
            var weather = city.Weathers?.First();
            var temp = city.Main?.GetTemperatureInCelsius() ?? default;
            var icon = weather?.Icon ?? default;

            var point = SphericalMercator.FromLonLat(city.Coordinate.Lon, city.Coordinate.Lat).ToMPoint();

            var feature = new PointFeature(point);

            feature["Name"] = city.Name;
            feature["Population"] = population;
            feature["Temperature"] = (int)Math.Round(temp);

            //feature.Styles.AddRange(LayerStyles.CreateLayerStyle(city, population, temp, icon));

            //     feature.Styles.AddRange(LayerStyles.CreateLayerStyle(city, population, temp, icon));

            return feature;
        }

        //private static IFeature CreateFeatureWithColors()
        //{
        //    var featureWithColors = new PointFeature(new MPoint(0, -7000000));
        //    featureWithColors.Styles.Add(LayerStyles.CreateColoredLabelStyle());
        //    return featureWithColors;
        //}

        //private static IFeature CreatePolygonWithLabel()
        //{
        //    var polygon = new GeometryFeature
        //    {
        //        Geometry = new WKTReader().Read(
        //            "POLYGON((-1000000 -10000000, 1000000 -10000000, 1000000 -8000000, -1000000 -8000000, -1000000 -10000000))")
        //    };
        //    polygon.Styles.Add(new LabelStyle
        //    {
        //        Text = "Polygon",
        //        BackColor = new Brush(Color.Gray),
        //        HorizontalAlignment = LabelStyle.HorizontalAlignmentEnum.Center
        //    });
        //    return polygon;
        //}

        //private static IFeature CreateFeatureWithWordWrapLeft()
        //{
        //    var featureWithColors = new PointFeature(new MPoint(-8000000, 6000000));
        //    featureWithColors.Styles.Add(new LabelStyle
        //    {
        //        Text = "Long line break mode test",
        //        Font = new Font { Size = 16, Bold = true, Italic = false, },
        //        BackColor = new Brush(Color.Gray),
        //        ForeColor = Color.White,
        //        Halo = new Pen(Color.Black, 2),
        //        MaxWidth = 10,
        //        LineHeight = 1.2,
        //        WordWrap = LabelStyle.LineBreakMode.WordWrap,
        //        HorizontalAlignment = LabelStyle.HorizontalAlignmentEnum.Left,
        //        VerticalAlignment = LabelStyle.VerticalAlignmentEnum.Top,
        //    });
        //    return featureWithColors;
        //}
    }
}
