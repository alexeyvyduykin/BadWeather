using BadWeather.ViewModels;

namespace BadWeather.Designer
{
    public class DesignTimeMainViewModel : MainViewModel
    {
        public static readonly DesignTimeData _designTimeData = new();

        public DesignTimeMainViewModel() : base(_designTimeData)
        {

        }
    }
}
