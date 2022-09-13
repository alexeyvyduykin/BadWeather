using Mapsui;
using ReactiveUI.Fody.Helpers;

namespace BadWeather.ViewModels
{
    public class FeatureTip : Tip
    {
        public void Update(IFeature? feature = null)
        {
            IsVisible = feature != null;

            if (feature == null)
            {
                Name = null;
                Temperature = null;
                Icon = null;
                return;
            }

            var temp = feature.GetValue<int?>("Temperature");
            var icon = feature.GetValue<string>("Icon");
            var tempStr = temp?.ToString("+#;-#;0");

            Name = feature.GetValue<string>("Name");
            Temperature = tempStr;
            Icon = icon;

            var isSelected = feature.GetValue<bool>("selected");

            if (isSelected == false)
            {
                IsExpanded = false;
                return;
            }

            IsExpanded = true;

            var pressure = feature.GetValue<double?>("Pressure");
            var humidity = feature.GetValue<int?>("Humidity");
            var cloudiness = feature.GetValue<int?>("Cloudiness");
            var degree = feature.GetValue<int?>("Degree");
            var speed = feature.GetValue<double?>("Speed");

            Pressure = $"{(int)Math.Round(pressure ?? 0.0 / 1.333)}";
            Humidity = $"{humidity}";
            Cloudiness = $"{cloudiness}";
            Degree = degree;
            Speed = speed;
        }

        [Reactive]
        public string? Name { get; set; }

        [Reactive]
        public string? Temperature { get; set; }

        [Reactive]
        public string? Icon { get; set; }

        [Reactive]
        public bool IsExpanded { get; set; }

        [Reactive]
        public string? Pressure { get; set; }

        [Reactive]
        public string? Humidity { get; set; }

        [Reactive]
        public string? Cloudiness { get; set; }

        [Reactive]
        public int? Degree { get; set; }

        [Reactive]
        public double? Speed { get; set; }
    }
}
