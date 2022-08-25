using Mapsui;
using ReactiveUI.Fody.Helpers;

namespace BadWeather.ViewModels
{
    public class FeatureTip : Tip
    {
        public FeatureTip()
        {

        }

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

            var temp = feature?.GetValue<int?>("Temperature");
            var icon = feature?.GetValue<string>("Icon");
            var tempStr = temp?.ToString("+#;-#;0");

            Name = feature?.GetValue<string>("Name");
            Temperature = temp != null ? $"{tempStr} °C" : string.Empty;
            Icon = icon != null ? new Uri($"resm:BadWeather.EmbeddedResources.{icon}@2x.png?assembly=BadWeather") : null;
        }

        [Reactive]
        public string? Name { get; set; }

        [Reactive]
        public string? Temperature { get; set; }

        [Reactive]
        public Uri? Icon { get; set; }
    }
}
