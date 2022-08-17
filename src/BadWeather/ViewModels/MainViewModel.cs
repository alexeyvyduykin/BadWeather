using BadWeather.Controllers;
using BadWeather.Models;
using BadWeather.Services.Cities;
using DynamicData;
using InteractiveGeometry;
using InteractiveGeometry.UI;
using Mapsui;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
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

            Tip = new DrawingTip();

            Tip.Creating("Area 4354.4545");

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

            var selector = (ISelector)new BaseSelector();

            selector.Selecting += (s, e) =>
            {
                if (s is IFeature feature)
                {
                    FeatureInfo = feature.ToFeatureInfo();
                }
            };

            selector.Unselecting += (s, e) =>
            {
                if (s is IFeature feature)
                {
                    FeatureInfo = feature.ToFeatureInfo();
                }
            };

            ActualController = new CustomController(selector);
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

        [Reactive]
        public DrawingTip Tip { get; set; }

        [Reactive]
        public string? FeatureInfo { get; set; }

        public Map Map => _map;

        public IController ActualController { get; set; }
    }
}
