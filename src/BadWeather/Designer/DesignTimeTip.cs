using BadWeather.ViewModels;

namespace BadWeather.Designer
{
    public class DesignTimeTip : DrawingTip
    {
        public DesignTimeTip() : base()
        {
            HoverCreating(34545.432);

            InvalidateVisual();
        }
    }
}
