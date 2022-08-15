using Avalonia;
using Mapsui;
using Mapsui.UI.Avalonia;
using System;

namespace BadWeather.Avalonia
{
    public class UserMapControl : MapControl
    {
        public UserMapControl() : base()
        {
            MapSourceProperty.Changed.Subscribe(OnMapSourceChanged);
        }

        public Map MapSource
        {
            get { return (Map)GetValue(MapSourceProperty); }
            set { SetValue(MapSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MapSource.  This enables animation, styling, binding, etc...
        public static readonly StyledProperty<Map> MapSourceProperty =
            AvaloniaProperty.Register<UserMapControl, Map>(nameof(MapSource));

        private static void OnMapSourceChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Sender is UserMapControl mapControl)
            {
                if (e.NewValue != null && e.NewValue is Map map)
                {
                    mapControl.Map = map;
                }
            }
        }
    }
}
