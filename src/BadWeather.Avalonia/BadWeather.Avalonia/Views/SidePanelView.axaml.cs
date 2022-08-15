using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace BadWeather.Avalonia.Views
{
    public partial class SidePanelView : UserControl
    {
        public SidePanelView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
