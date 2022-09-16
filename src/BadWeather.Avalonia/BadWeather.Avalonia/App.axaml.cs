using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using BadWeather.Avalonia.Views;
using BadWeather.ViewModels;
using Splat;

namespace BadWeather.Avalonia
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private static T GetExistingService<T>() => Locator.Current.GetExistingService<T>();

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                RegisterBootstrapper(desktop);

                var mainViewModel = GetExistingService<MainViewModel>();

                desktop.MainWindow = new MainWindow
                {
                    DataContext = mainViewModel
                };
            }
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            {
                RegisterBootstrapper(singleViewPlatform);

                var mainViewModel = GetExistingService<MainViewModel>();

                singleViewPlatform.MainView = new MainView
                {
                    DataContext = mainViewModel
                };
            }

            base.OnFrameworkInitializationCompleted();
        }

        private static void RegisterBootstrapper(IApplicationLifetime lifetime)
        {
            // I only want to hear about errors
            var logger = new ConsoleLogger() { Level = Splat.LogLevel.Error };
            Locator.CurrentMutable.RegisterConstant(logger, typeof(ILogger));

            if (lifetime is IClassicDesktopStyleApplicationLifetime)
            {
                Bootstrapper.RegisterDesktop(Locator.CurrentMutable, Locator.Current);
            }
            else if (lifetime is ISingleViewApplicationLifetime)
            {
                Bootstrapper.RegisterWeb(Locator.CurrentMutable, Locator.Current);
            }
        }
    }
}