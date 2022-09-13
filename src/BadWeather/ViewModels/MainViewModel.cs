using BadWeather.Services.Cities;
using BadWeather.Services.OpenWeather;
using DynamicData;
using Mapsui;
using Mapsui.Interactivity;
using Mapsui.Interactivity.UI;
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
        private readonly SourceList<CityViewModel> _cities = new();
        private readonly ReadOnlyObservableCollection<CityViewModel> _items;
        private readonly ObservableAsPropertyHelper<bool> _isLoading;
        private readonly IReadonlyDependencyResolver _dependencyResolver;
        private readonly Map _map;
        private readonly CitiesDataService _citiesDataService;
        private readonly CityProvider _provider;

        public MainViewModel(IReadonlyDependencyResolver dependencyResolver)
        {
            _dependencyResolver = dependencyResolver;

            _map = (Map)dependencyResolver.GetExistingService<IMap>();
            _citiesDataService = dependencyResolver.GetExistingService<CitiesDataService>();
            var openWeatherService = dependencyResolver.GetExistingService<OpenWeatherService>();

            _provider = new CityProvider(_citiesDataService, openWeatherService);

            Tip = new FeatureTip();

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

            var selector = new BaseSelector();

            selector.Select += (s, e) =>
            {
                if (s is IFeature feature)
                {
                    FeatureInfo = feature.ToFeatureInfo();
                    Tip.Update(feature);
                }
            };

            selector.Unselect += (s, e) =>
            {
                if (s is IFeature feature)
                {
                    FeatureInfo = feature.ToFeatureInfo();
                    Tip.Update(feature);
                }
            };

            selector.HoveringBegin += (s, e) =>
            {
                if (s is IFeature feature)
                {
                    Tip.Update(feature);
                }
            };

            selector.HoveringEnd += (s, e) =>
            {
                Tip.Update(null);
            };

            Behavior = new InteractiveBehavior(selector);

            ActualController = new SelectingController();
        }

        public ReactiveCommand<Unit, Unit> Loading { get; }

        public bool IsLoading => _isLoading.Value;

        private async Task LoadingImpl()
        {
            var list = await _provider.GetCitiesAsync();

            _cities.Edit(innerList =>
            {
                innerList.Clear();
                innerList.AddRange(list);
            });
        }

        public ReadOnlyObservableCollection<CityViewModel> Cities => _items;

        [Reactive]
        public CityViewModel? SelectedCity { get; set; }

        [Reactive]
        public FeatureTip Tip { get; set; }

        [Reactive]
        public string? FeatureInfo { get; set; }

        public Map Map => _map;

        public IController ActualController { get; set; }

        public IInteractiveBehavior Behavior { get; set; }
    }
}
