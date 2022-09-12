using Avalonia.ReactiveUI;
using BadWeather.ViewModels;
using ReactiveUI;

namespace BadWeather.Avalonia.Views
{
    public partial class TipView : ReactiveUserControl<FeatureTip>
    {
        public TipView()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {

            });
        }
    }
}
