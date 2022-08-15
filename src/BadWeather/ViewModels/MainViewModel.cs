using BadWeather.Models;
using BadWeather.Services.Cities;
using DynamicData;
using Mapsui;
using ReactiveUI;
using Splat;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;

namespace BadWeather.ViewModels
{
    public class MainViewModel : ReactiveObject
    {
        private readonly SourceList<City> _cities = new();
        private readonly ReadOnlyObservableCollection<City> _items;
        private readonly ObservableAsPropertyHelper<bool> _isLoading;
        private readonly IReadonlyDependencyResolver _dependencyResolver;
        private readonly Map _map;
        private readonly CitiesDataService _citiesDataService;

        public MainViewModel(IReadonlyDependencyResolver dependencyResolver)
        {
            _dependencyResolver = dependencyResolver;

            _map = (Map)dependencyResolver.GetExistingService<IMap>();
            _citiesDataService = dependencyResolver.GetExistingService<CitiesDataService>();

            _cities
                .Connect()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out _items)
                .Subscribe();

            Loading = ReactiveCommand.CreateFromTask(LoadingImpl);

            _isLoading = Loading
                .IsExecuting
                .ObserveOn(RxApp.MainThreadScheduler)
                .ToProperty(this, x => x.IsLoading);

            Loading.Execute().Subscribe();
        }

        public ReactiveCommand<Unit, Unit> Loading { get; }

        public bool IsLoading => _isLoading.Value;

        private async Task LoadingImpl()
        {
            var list = await _citiesDataService.GetCitiesAsync();

            _cities.Edit(innerList =>
            {
                innerList.Clear();
                innerList.AddRange(list);
            });
        }

        public ReadOnlyObservableCollection<City> Cities => _items;

        public Map Map => _map;
    }
}
