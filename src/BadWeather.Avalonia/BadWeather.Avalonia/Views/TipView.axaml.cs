using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.ReactiveUI;
using BadWeather.ViewModels;
using ReactiveUI;
using System;
using System.Reactive.Disposables;

namespace BadWeather.Avalonia.Views
{
    public partial class TipView : ReactiveUserControl<FeatureTip>
    {
        public TipView()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, vm => vm.Icon, v => v.ImageIcon.Source, s => Convert(s)).DisposeWith(disposables);
            });
        }

        private static object? Convert(Uri? uri)
        {
            if (uri == null)
            {
                return null;
            }

            var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
            var asset = assets?.Open(uri);
            return new Bitmap(asset);
        }
    }
}
