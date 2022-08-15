using BadWeather.Services.Cities;
using BadWeather.ViewModels;
using Splat;

namespace BadWeather.Avalonia
{
    public static class Bootstrapper
    {
        public static void Register(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
        {
            services.InitializeSplat();

            RegisterConfigurations(services, resolver);
            RegisterViewModels(services, resolver);
        }

        private static void RegisterConfigurations(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
        {
            //var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        }

        private static void RegisterViewModels(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
        {
            services.Register(() => new ViewModelFactory(resolver));
            services.Register(() => new MapFactory(resolver));

            var mapFactory = resolver.GetExistingService<MapFactory>();
            var viewModelFactory = resolver.GetExistingService<ViewModelFactory>();

            services.RegisterConstant<CitiesDataService>(new CitiesDataService());
            services.RegisterConstant(mapFactory.CreateMap(), typeof(Mapsui.IMap));

            services.RegisterLazySingleton<MainViewModel>(() => new MainViewModel(resolver));
        }
    }
}
