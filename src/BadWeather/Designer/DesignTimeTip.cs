using BadWeather.ViewModels;
using Mapsui.Layers;

namespace BadWeather.Designer
{
    public class DesignFeatureTip : FeatureTip
    {
        public DesignFeatureTip() : base()
        {
            var feature = new PointFeature(0, 0);

            feature["Name"] = "London";
            feature["Temperature"] = 17;
            feature["Icon"] = "02d";

            Update(feature);
        }
    }
}
