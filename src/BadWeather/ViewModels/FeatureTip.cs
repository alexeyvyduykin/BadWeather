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

            var temp = feature?.GetValue<int?>("Temperature");
            var icon = feature?.GetValue<string>("Icon");
            var tempStr = temp?.ToString("+#;-#;0");

            Name = feature?.GetValue<string>("Name");
            Temperature = temp != null ? $"{tempStr} °C" : string.Empty;
            Icon = icon;
        }

        [Reactive]
        public string? Name { get; set; }

        [Reactive]
        public string? Temperature { get; set; }

        [Reactive]
        public string? Icon { get; set; }
    }
}
