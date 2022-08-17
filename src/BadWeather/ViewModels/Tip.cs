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

    public class DrawingTip : Tip
    {
        private object _title = default(bool);
        private object _text = default(bool);

        public DrawingTip()
        {
            Value = null;

            InvalidateVisual();
        }

        public object TitleDirty => _title;

        public object TextDirty => _text;

        protected void InvalidateVisual()
        {
            this.RaiseAndSetIfChanged(ref _title, !(bool)_title, nameof(TitleDirty));
            this.RaiseAndSetIfChanged(ref _text, !(bool)_text, nameof(TextDirty));
        }

        public object? Value { get; private set; }

        public void HoverCreating(object? value = null)
        {
            Value = value;

            InvalidateVisual();
        }

        public void BeginCreating(object? value = null)
        {
            Value = value;

            InvalidateVisual();
        }

        public void Creating(object? value = null)
        {
            Value = value;

            InvalidateVisual();
        }
    }
}
