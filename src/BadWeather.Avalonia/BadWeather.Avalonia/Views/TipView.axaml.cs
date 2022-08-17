using Avalonia.ReactiveUI;
using BadWeather.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;

namespace BadWeather.Avalonia.Views
{
    public partial class TipView : ReactiveUserControl<DrawingTip>
    {
        public TipView()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, vm => vm.TextDirty, v => v.Title.Text, _ => ConvertToTitle(ViewModel)).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.TitleDirty, v => v.Text.Text, _ => ConvertToText(ViewModel)).DisposeWith(disposables);
            });
        }
        private static string ConvertToTitle(DrawingTip? tip)
        {
            if (tip == null)
            {
                return string.Empty;
            }

            var value = tip.Value;

            return $"{value}";
        }

        private static string ConvertToText(DrawingTip? tip)
        {
            if (tip == null)
            {
                return string.Empty;
            }

            var value = tip.Value;

            return $"{value}";
        }
    }
}
