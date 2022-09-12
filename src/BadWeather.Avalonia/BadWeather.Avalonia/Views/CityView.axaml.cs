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
    }
}
