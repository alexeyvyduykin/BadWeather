using BadWeather.Services.OpenWeather;
using Mapsui.Extensions;
using Mapsui.Styles;

namespace BadWeather.Styles
{
    public static class LayerStyles
    {
        public static double GetMaxVisible(int population)
        {
            //var population = city.Population;

            double maxVisible = 0;

            if (population > 1000000)
            {
                maxVisible = 40000;
            }
            else if (population > 500000 && population < 1000000)
            {
                maxVisible = 18000;
            }
            else if (population > 300000 && population < 500000)
            {
                maxVisible = 3000;
            }
            else if (population > 100000 && population < 300000)
            {
                maxVisible = 1800;
            }

            return maxVisible;
        }

        public static IEnumerable<IStyle> CreateLayerStyle(OpenWeatherModel city, int population, double temperature, string icon)
        {
            return new[]
            {
                LayerStyles.CreatePointStyle(city, population),
                LayerStyles.CreateLabel1Style(city, population, temperature),
                LayerStyles.CreateLabel2Style(city, population, temperature),
                LayerStyles.CreateBitmapStyle(city, population, icon),
            };
        }

        private static IStyle CreatePointStyle(OpenWeatherModel city, int population)
        {
            return new SymbolStyle
            {
                SymbolType = SymbolType.Ellipse,
                Fill = new Brush(Color.Transparent),
                Line = new Pen(Color.Black, 4),
                Outline = new Pen(Color.Black, 4),
                SymbolScale = 0.2,
                MaxVisible = GetMaxVisible(population),
            };
        }

        private static IStyle CreateLabel1Style(OpenWeatherModel city, int population, double temp)
        {
            temp = Math.Round(temp);

            return new LabelStyle
            {
                Text = $".    {temp.ToString("+0;-#")}",
                Font = new Font { Size = 16, Bold = false, Italic = false, },
                BackColor = new Brush(Color.Black),
                ForeColor = Color.White,
                CornerRounding = 10,
                Offset = new Offset(0, -15),
                WordWrap = LabelStyle.LineBreakMode.WordWrap,
                //     Halo = new Pen(Color.Black, 2),
                //     MaxWidth = 10,
                //     LineHeight = 1.2,
                //     WordWrap = LabelStyle.LineBreakMode.WordWrap,
                //  HorizontalAlignment = LabelStyle.HorizontalAlignmentEnum.Left,
                // VerticalAlignment = LabelStyle.VerticalAlignmentEnum.Center,
                MaxVisible = LayerStyles.GetMaxVisible(population),
            };
        }

        private static IStyle CreateLabel2Style(OpenWeatherModel city, int population, double temp)
        {
            temp = Math.Round(temp);

            return new LabelStyle
            {
                Text = $"{city.Name}",//: {temp}°C",
                Font = new Font { Size = 16, Bold = false, Italic = false, },
                BackColor = new Brush(Color.Transparent),
                //BackColor = new Brush(Color.Gray),
                ForeColor = Color.Black,
                CornerRounding = 0,
                //     LineHeight = 5.0,
                Offset = new Offset(0, +12),
                WordWrap = LabelStyle.LineBreakMode.WordWrap,
                //     Halo = new Pen(Color.Black, 2),
                //     MaxWidth = 10,
                //     LineHeight = 1.2,
                //     WordWrap = LabelStyle.LineBreakMode.WordWrap,
                //  HorizontalAlignment = LabelStyle.HorizontalAlignmentEnum.Left,
                // VerticalAlignment = LabelStyle.VerticalAlignmentEnum.Center,
                MaxVisible = LayerStyles.GetMaxVisible(population),
            };
        }

        private static IStyle CreateBitmapStyle(OpenWeatherModel city, int population, string icon)
        {
            // For this sample we get the bitmap from an embedded resouce
            // but you could get the data stream from the web or anywhere
            // else.

            var bitmapId = typeof(MapFactory).LoadBitmapId($"EmbeddedResources.{icon}@2x.png");
            var bitmapHeight = 176; // To set the offset correct we need to know the bitmap height
            return new SymbolStyle
            {
                BitmapId = bitmapId,
                SymbolScale = 0.30,
                SymbolOffset = new Offset(-45, 45),
                // SymbolOffset = new Offset(0, bitmapHeight * 0.5),
                MaxVisible = GetMaxVisible(population),
            };
        }

        //public static IStyle CreateColoredLabelStyle()
        //{
        //    return new LabelStyle
        //    {
        //        Text = "Colors",
        //        BackColor = new Brush(Color.Blue),
        //        ForeColor = Color.White
        //    };
        //}

        //public static SymbolStyle CreateBitmapStyle()
        //{
        //    // For this sample we get the bitmap from an embedded resouce
        //    // but you could get the data stream from the web or anywhere
        //    // else.
        //    var bitmapId = typeof(MapFactory).LoadBitmapId(@"EmbeddedResources.home.png");
        //    var bitmapHeight = 176; // To set the offset correct we need to know the bitmap height
        //    return new SymbolStyle
        //    {
        //        BitmapId = bitmapId,
        //        SymbolScale = 0.20,
        //        SymbolOffset = new Offset(0, bitmapHeight * 0.5)
        //    };
        //}

        //public static SymbolStyle CreateBitmapMarkerStyle(OpenWeatherModel city, int population)
        //{
        //    // For this sample we get the bitmap from an embedded resouce
        //    // but you could get the data stream from the web or anywhere
        //    // else.
        //    var bitmapId = typeof(MapFactory).LoadBitmapId(@"EmbeddedResources.marker.png");
        //    var bitmapHeight = 64; // To set the offset correct we need to know the bitmap height
        //    return new SymbolStyle
        //    {
        //        BitmapId = bitmapId,
        //        SymbolScale = 0.85,
        //        SymbolOffset = new Offset(0, bitmapHeight * 0.5),
        //        MaxVisible = GetMaxVisible(population),
        //    };
        //}
    }
}
