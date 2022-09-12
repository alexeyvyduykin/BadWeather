﻿using BadWeather.Services.OpenWeather;
using Mapsui.Layers;
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

        public static IEnumerable<IStyle> CreateLayerStyle(OpenWeatherModel city, int population, double temperature, string icon)
        {
            return new[]
            {
                //CreateLabel_Style(),
               //CreatePointStyle(population),
   //            CreateLabel1Style(population, (int)Math.Round(temperature)),
               // LayerStyles.CreatePointStyle2(),
               // LayerStyles.CreateLabel1Style(),
                CreateLabel2Style(city.Name!, population),               
     //           CreateBitmapStyle(city, population, icon),
            };
        }

        private static IStyle CreatePointStyle(int population)
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

        public static IStyle CreatePointStyle2()
        {
            var stl = new ThemeStyle(f =>
            {
                if (f is not PointFeature pf)
                {
                    return null;
                }

                int? population = null;

                if (f.Fields.Contains("Population"))
                {
                    population = (int)f["Population"]!;
                }

                var style = new SymbolStyle
                {
                    SymbolType = SymbolType.Ellipse,
                    Fill = new Brush(Color.Transparent),
                    Line = new Pen(Color.Black, 4),
                    Outline = new Pen(Color.Black, 4),
                    SymbolScale = 0.2,
                    MaxVisible = GetMaxVisible(population ?? int.MaxValue),
                };

                if (pf.Fields.Contains("pointerover"))
                {
                    if ((bool)pf["pointerover"]! == true)
                    {
                        style.Fill = new Brush(Color.Red);
                        style.SymbolScale = 0.4;

                        return style;
                    }
                }

                return style;
            });

            return stl;
        }

        public static IStyle CreateTopLabelStyle()
        {
            return new ThemeStyle(f =>
            {
                var population = f.GetValue<int?>("Population");
                var temperature = f.GetValue<int?>("Temperature");
                var icon = f.GetValue<string>("Icon");
                var maxVisible = GetMaxVisible(population ?? int.MaxValue);

                var text = temperature?.ToString("+0;-#");

                return new LabelStyle
                {
                    Text = $"{text}",
                    Font = new Font { Size = 16, Bold = false, Italic = false, },
                    BackColor = new Brush(Color.FromArgb(1, 1, 1, 1)),
                    //BorderColor = Color.FromArgb(255, 1, 1, 1),
                    //BorderThickness = 2,
                    ForeColor = Color.Black,
                    //Halo = new Pen(Color.Black, 1),
                    //CornerRounding = 2,
                    Offset = new Offset(0, -15),
                    HorizontalAlignment = LabelStyle.HorizontalAlignmentEnum.Left,
                    //WordWrap = LabelStyle.LineBreakMode.WordWrap,
                    MaxVisible = maxVisible,
                };
            });
        }

        public static IStyle CreateBottomLabelStyle()
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
                    CornerRounding = 0,
                    Offset = new Offset(0, +12),

                    //WordWrap = LabelStyle.LineBreakMode.WordWrap,
                    MaxVisible = maxVisible,
                };
            });
        }


        public static IStyle CreateLabel_Style()
        {
            var col = new StyleCollection();

            var stl1 = new ThemeStyle(f =>
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

            var stl2 = new ThemeStyle(f =>
            {
                var population = f.GetValue<int?>("Population");
                var temperature = f.GetValue<int?>("Temperature");
                var pointerover = f.GetValue<bool>("pointerover");
                var maxVisible = GetMaxVisible(population ?? int.MaxValue);

                return new LabelStyle
                {
                    Text = $".    {temperature?.ToString("+0;-#")}",
                    Font = new Font { Size = 16, Bold = false, Italic = false, },
                    BackColor = new Brush(Color.Black),
                    ForeColor = Color.White,
                    CornerRounding = 10,
                    Offset = new Offset(0, -15),
                    WordWrap = LabelStyle.LineBreakMode.WordWrap,
                    MaxVisible = maxVisible,
                };
            });

            var stl3 = new ThemeStyle(f =>
            {
                var population = f.GetValue<int?>("Population");
                var name = f.GetValue<string>("Name");
                var maxVisible = GetMaxVisible(population ?? int.MaxValue);

                return new LabelStyle
                {
                    Text = $"{name}",
                    Font = new Font { Size = 18, Bold = false, Italic = false, },
                    BackColor = new Brush(Color.Transparent),
                    ForeColor = Color.Black,
                    CornerRounding = 0,
                    Offset = new Offset(0, +12),
                    WordWrap = LabelStyle.LineBreakMode.WordWrap,
                    MaxVisible = maxVisible,
                };
            });

            col.Add(stl1);
            //  col.Add(stl2);
            // col.Add(stl3);

            return col;
        }

        public static IStyle CreateLabel1Style(int population, int temperature)
        {
            return new LabelStyle
            {
                Text = $".    {temperature.ToString("+0;-#")}",
                Font = new Font { Size = 16, Bold = false, Italic = false, },
                BackColor = new Brush(Color.Black),
                ForeColor = Color.White,
                CornerRounding = 10,
                Offset = new Offset(0, -15),
                //WordWrap = LabelStyle.LineBreakMode.WordWrap,
                //     Halo = new Pen(Color.Black, 2),
                //     MaxWidth = 10,
                //     LineHeight = 1.2,
                //     WordWrap = LabelStyle.LineBreakMode.WordWrap,
                //  HorizontalAlignment = LabelStyle.HorizontalAlignmentEnum.Left,
                // VerticalAlignment = LabelStyle.VerticalAlignmentEnum.Center,
                MaxVisible = GetMaxVisible(population),
            };
        }

        public static IStyle CreateLabel2Style(string name, int population)
        {
            return new LabelStyle
            {
                Text = $"{name}",
                Font = new Font { Size = 18, Bold = false, Italic = false, },
                BackColor = new Brush(Color.Transparent),
                //BackColor = new Brush(Color.Gray),
                ForeColor = Color.Black,
                //CornerRounding = 0,
                //     LineHeight = 5.0,
                Offset = new Offset(0, +12),
                //WordWrap = LabelStyle.LineBreakMode.WordWrap,
                //Halo = new Pen(Color.Black, 1),
                //     MaxWidth = 10,
                //     LineHeight = 1.2,
                //     WordWrap = LabelStyle.LineBreakMode.WordWrap,
                //  HorizontalAlignment = LabelStyle.HorizontalAlignmentEnum.Left,
                // VerticalAlignment = LabelStyle.VerticalAlignmentEnum.Center,
                MaxVisible = GetMaxVisible(population),
            };
        }
    }
}
