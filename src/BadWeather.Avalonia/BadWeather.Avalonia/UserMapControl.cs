using Avalonia.Controls;
using Avalonia.Input;
using BadWeather.Avalonia.Views;
using BadWeather.ViewModels;
using Mapsui.Interactivity.UI.Avalonia;

namespace BadWeather.Avalonia
{
    public class UserMapControl : InteractiveMapControl
    {
        private ITip? _tip;

        public UserMapControl() : base() { }

        public override void ApplyTemplate()
        {
            base.ApplyTemplate();

            foreach (var item in Children)
            {
                if (item is Canvas canvas)
                {
                    foreach (var item2 in canvas.Children)
                    {
                        if (item2 is TipView tipView)
                        {
                            _tip = (ITip)tipView.DataContext!;
                        }
                    }
                }
            }
        }

        protected override void OnPointerMoved(PointerEventArgs e)
        {
            base.OnPointerMoved(e);

            if (_tip != null)
            {
                var screenPosition = e.GetPosition(this);

                _tip.X = screenPosition.X + 20;
                _tip.Y = screenPosition.Y;
            }
        }
    }
}
