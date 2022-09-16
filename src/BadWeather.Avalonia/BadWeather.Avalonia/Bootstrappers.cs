using Avalonia;
using Avalonia.Platform;
using BadWeather.Services;
using BadWeather.Services.OpenWeather;
using BadWeather.ViewModels;
using Splat;
using System;
using System.IO;

namespace BadWeather.Avalonia
{
    public static class Bootstrapper
    {
        public static void RegisterDesktop(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
        {
            services.InitializeSplat();

            RegisterConfigurations(services, resolver);
            RegisterViewModels(services, resolver);
        }

        public static void RegisterWeb(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
        {
            services.InitializeSplat();

            RegisterConfigurations(services, resolver);
            RegisterViewModelsWeb(services, resolver);
        }

        private static void RegisterConfigurations(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
        {
            //var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        }

        private static void RegisterViewModelsWeb(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
        {
            var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();

            var uri1 = new Uri("avares://BadWeather.Avalonia/Assets/cities60.json");
            var uri2 = new Uri("avares://BadWeather.Avalonia/Assets/openweatherCities60.json");

            services.Register(() => new ViewModelFactory(resolver));
            services.Register(() => new MapFactory(resolver));

            var mapFactory = resolver.GetExistingService<MapFactory>();
            var viewModelFactory = resolver.GetExistingService<ViewModelFactory>();

            var dataService = new DataService();

            dataService.RegisterSource("city", assets?.Open(uri1));
            dataService.RegisterSource("openweatherCity", assets?.Open(uri2));

            services.RegisterConstant<DataService>(dataService);
            services.RegisterConstant<OpenWeatherService>(new OpenWeatherService("fd8fe7226da08b68880c55c35f7ea4f8"));
            services.RegisterLazySingleton<Mapsui.IMap>(() => mapFactory.CreateMap("bing"));

            services.RegisterLazySingleton<MainViewModel>(() => new MainViewModel(resolver));
        }

        private static void RegisterViewModels(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
        {
            var runningPath = AppDomain.CurrentDomain.BaseDirectory;

            var path1 = string.Format("{0}src\\BadWeather.Avalonia\\BadWeather.Avalonia\\Assets\\cities60.json", Path.GetFullPath(Path.Combine(runningPath, @"..\..\..\..\..\..\")));
            var path2 = string.Format("{0}src\\BadWeather.Avalonia\\BadWeather.Avalonia\\Assets\\openweatherCities60.json", Path.GetFullPath(Path.Combine(runningPath, @"..\..\..\..\..\..\")));

            services.Register(() => new ViewModelFactory(resolver));
            services.Register(() => new MapFactory(resolver));

            var mapFactory = resolver.GetExistingService<MapFactory>();
            var viewModelFactory = resolver.GetExistingService<ViewModelFactory>();

            var dataService = new DataService();

            dataService.RegisterSource("city", path1);
            dataService.RegisterSource("openweatherCity", path2);

            services.RegisterConstant<DataService>(dataService);
            services.RegisterConstant<OpenWeatherService>(new OpenWeatherService("fd8fe7226da08b68880c55c35f7ea4f8"));
            services.RegisterLazySingleton<Mapsui.IMap>(() => mapFactory.CreateMap("world"));

            services.RegisterLazySingleton<MainViewModel>(() => new MainViewModel(resolver));
        }
    }
}
