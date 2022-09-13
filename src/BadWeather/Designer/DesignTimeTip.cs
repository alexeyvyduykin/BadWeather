using BadWeather.ViewModels;
using Mapsui.Layers;

namespace BadWeather.Designer
{
    public class DesignFeatureTip : FeatureTip
    {
        public DesignFeatureTip() : base()
        {
            var feature = new PointFeature(0, 0);

            feature["selected"] = true;
            feature["Name"] = "London";
            feature["Temperature"] = 17;
            feature["Icon"] = "02d";
            feature["Pressure"] = 850.45;
            feature["Humidity"] = 75;
            feature["Cloudiness"] = 25;
            feature["Degree"] = 45;
            feature["Speed"] = 3.5;

            Update(feature);
        }
    }
}
