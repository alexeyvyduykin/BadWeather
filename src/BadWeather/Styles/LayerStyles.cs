using Mapsui.Styles;
using Mapsui.Styles.Thematics;

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

        public static IStyle CreateTemperatureLabel()
        {
            return new ThemeStyle(f =>
            {
                var population = f.GetValue<int?>("Population");
                var temperature = f.GetValue<int?>("Temperature");
                var maxVisible = GetMaxVisible(population ?? int.MaxValue);

                var text = temperature?.ToString("+0;-#");

                return new LabelStyle
                {
                    Text = $"{text}",
                    Font = new Font { Size = 16, Bold = false, Italic = false, },
                    BackColor = new Brush(Color.FromArgb(1, 1, 1, 1)),
                    ForeColor = Color.Black,
                    Offset = new Offset(0, -15),
                    HorizontalAlignment = LabelStyle.HorizontalAlignmentEnum.Left,
                    MaxVisible = maxVisible,
                };
            });
        }

        public static IStyle CreateCityLabel()
        {
            return new ThemeStyle(f =>
            {
                var population = f.GetValue<int?>("Population");
                var name = f.GetValue<string>("Name");
                var maxVisible = GetMaxVisible(population ?? int.MaxValue);
                var isSelected = f.GetValue<bool>("selected");

                return new LabelStyle
                {
                    Text = $"{name}",
                    Font = (isSelected == false)
                    ? new Font { Size = 18, Bold = false, Italic = false, }
                    : new Font { Size = 20, Bold = true, Italic = false, },
                    BackColor = new Brush(Color.FromArgb(1, 1, 1, 1)),
                    ForeColor = Color.Black,
                    Offset = new Offset(0, +12),
                    MaxVisible = maxVisible,
                };
            });
        }

        public static IStyle CreatePointStyle()
        {
            return new ThemeStyle(f =>
            {
                var population = f.GetValue<int?>("Population");
                var pointerover = f.GetValue<bool>("pointerover");
                var maxVisible = GetMaxVisible(population ?? int.MaxValue);

                return new SymbolStyle
                {
                    SymbolType = SymbolType.Ellipse,
                    Fill = pointerover == false ? new Brush(Color.Transparent) : new Brush(Color.Red),
                    Line = new Pen(Color.Black, 4),
                    Outline = new Pen(Color.Black, 4),
                    SymbolScale = pointerover == false ? 0.2 : 0.4,
                    MaxVisible = maxVisible,
                };
            });
        }
    }
}
