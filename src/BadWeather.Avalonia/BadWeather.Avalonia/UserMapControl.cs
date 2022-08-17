using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using BadWeather.ViewModels;
using InteractiveGeometry.UI.Avalonia;
using Mapsui;
using System;
using System.Collections.ObjectModel;

namespace BadWeather.Avalonia
{
    public class UserMapControl : InteractiveMapControl
    {
        private readonly ItemsControl? _tipControl;

        public UserMapControl() : base()
        {
            TipSourceProperty.Changed.Subscribe(OnTipSourceChanged);
            MapSourceProperty.Changed.Subscribe(OnMapSourceChanged);

            var itemsControl = CreateTip();

            if (itemsControl != null)
            {
                _tipControl = itemsControl;
                Children.Add(itemsControl);
            }
        }

        private static ItemsControl? CreateTip()
        {
            string xaml = @"
          <ItemsControl xmlns='https://github.com/avaloniaui'
                        xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'
                        xmlns:views='clr-namespace:BadWeather.Avalonia.Views'>

          <ItemsControl.Styles>
            <Style Selector='ItemsControl > ContentPresenter'>
              <Setter Property='Canvas.Left' Value='{Binding X}'/>   
              <Setter Property='Canvas.Top' Value='{Binding Y}'/>      
              <Setter Property='IsVisible' Value='{Binding IsVisible}'/>      
            </Style>      
          </ItemsControl.Styles>
      
                <ItemsControl.ItemsPanel>
                    <ItemsPanelTemplate>
                        <Canvas Background='Transparent'/>
                    </ItemsPanelTemplate>
                </ItemsControl.ItemsPanel>
        
      <ItemsControl.ItemTemplate>
        <DataTemplate>
          <views:TipView DataContext='{Binding}'/>
        </DataTemplate>
      </ItemsControl.ItemTemplate>

</ItemsControl>";

            return AvaloniaRuntimeXamlLoader.Parse<ItemsControl>(xaml);
        }

        public ITip? TipSource
        {
            get { return GetValue(TipSourceProperty); }
            set { SetValue(TipSourceProperty, value); }
        }

        public static readonly StyledProperty<ITip?> TipSourceProperty =
            AvaloniaProperty.Register<UserMapControl, ITip?>(nameof(TipSource), null);

        private static void OnTipSourceChanged(AvaloniaPropertyChangedEventArgs e)
        {
            var mapControl = (UserMapControl)e.Sender;
            if (e.NewValue == null)
            {
                mapControl.HideTip();
            }
            else
            {
                mapControl.ShowTip();
            }
        }

        protected void ShowTip()
        {
            if (_tipControl != null && TipSource != null)
            {
                _tipControl.Items = new ObservableCollection<ITip>() { TipSource };
            }
        }

        protected void HideTip()
        {
            if (_tipControl != null)
            {
                _tipControl.Items = new ObservableCollection<ITip>();
            }
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

        protected override void OnPointerMoved(PointerEventArgs e)
        {
            base.OnPointerMoved(e);

            if (TipSource != null)
            {
                var screenPosition = e.GetPosition(this);

                TipSource.X = screenPosition.X + 20;
                TipSource.Y = screenPosition.Y;

                if (TipSource.IsVisible == false)
                {
                    TipSource.IsVisible = true;
                }
            }
        }
    }
}
