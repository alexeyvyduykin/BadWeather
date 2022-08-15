using Avalonia.ReactiveUI;
using Avalonia.Web.Blazor;

namespace BadWeather.Avalonia.Web
{
    public partial class App
    {
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            WebAppBuilder.Configure<BadWeather.Avalonia.App>()
                .UseReactiveUI()
                .SetupWithSingleViewLifetime();
        }
    }
}