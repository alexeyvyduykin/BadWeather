using Avalonia.ReactiveUI;
using BadWeather.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;

namespace BadWeather.Avalonia.Views
{
    public partial class SidePanelView : ReactiveUserControl<MainViewModel>
    {
        public SidePanelView()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, vm => vm.SelectedCity, v => v.CityInfoView.IsVisible, s => s != null).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.SelectedCity, v => v.CityInfoView.DataContext).DisposeWith(disposables);
            });
        }
    }
}
