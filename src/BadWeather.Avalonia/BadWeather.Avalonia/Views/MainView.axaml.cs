using Avalonia.ReactiveUI;
using BadWeather.ViewModels;

namespace BadWeather.Avalonia.Views
{
    public partial class MainView : ReactiveUserControl<MainViewModel>
    {
        public MainView()
        {
            InitializeComponent();
        }
    }
}