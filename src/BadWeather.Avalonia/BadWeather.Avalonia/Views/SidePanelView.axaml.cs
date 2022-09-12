using Avalonia.ReactiveUI;
using BadWeather.ViewModels;
using ReactiveUI;

namespace BadWeather.Avalonia.Views
{
    public partial class SidePanelView : ReactiveUserControl<MainViewModel>
    {
        public SidePanelView()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {

            });
        }
    }
}
