using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace BadWeather.ViewModels
{
    public interface ITip
    {
        double X { get; set; }

        double Y { get; set; }

        bool IsVisible { get; set; }
    }

    public class Tip : ReactiveObject, ITip
    {
        [Reactive]
        public double X { get; set; }

        [Reactive]
        public double Y { get; set; }

        [Reactive]
        public bool IsVisible { get; set; }
    }
}
