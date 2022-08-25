using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.ReactiveUI;
using BadWeather.ViewModels;
using ReactiveUI;
using System;
using System.Reactive.Disposables;

namespace BadWeather.Avalonia.Views
{
    public partial class CityView : ReactiveUserControl<CityViewModel>
    {
        public CityView()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, vm => vm.Icon, v => v.ImageIcon.Source, icon => Convert(icon)).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.Temperature, v => v.TemperatureTextBlock.Text, s => ConvertToTemperature(s)).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.Pressure, v => v.PressureTextBlock.Text, s => ConvertToPressure(s)).DisposeWith(disposables);
            });
        }

        private static object ConvertToTemperature(double value)
        {
            var res = ((int)Math.Round(value)).ToString("+#;-#;0");
            return $"{res} °C";
        }

        private static object ConvertToPressure(double value)
        {
            return $"{(int)Math.Round(value / 1.333)} mmHg";
        }

        private static object Convert(string icon)
        {
            //  return $"/Assets/avalonia-logo.ico";
            //  var uri = new Uri($"/Assets/avalonia-logo.ico");
            //  return $"resm:BadWeather.EmbeddedResources.{icon}.png?assembly=BadWeather";

            //return $"resm:BadWeather.EmbeddedResources.04d@2x.png?assembly=BadWeather";

            // icon = "04d";

            var uri = new Uri($"resm:BadWeather.EmbeddedResources.{icon}@2x.png?assembly=BadWeather");

            //var uri = new Uri($"avares://BadWeather.EmbeddedResources.{icon}.png;assembly=BadWeather");
            var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
            var asset = assets?.Open(uri);
            return new Bitmap(asset);
        }
    }
}
