using BadWeather.ViewModels;

namespace BadWeather.Designer
{
    public class DesignTimeCityViewModel : CityViewModel
    {
        public DesignTimeCityViewModel() : base(DesignTimeData.BuildOpenWeatherModel())
        {

        }
    }
}
