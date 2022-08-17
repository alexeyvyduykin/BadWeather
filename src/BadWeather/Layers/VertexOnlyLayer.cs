using Mapsui;
using Mapsui.Layers;

namespace BadWeather.Layers
{
    public class VertexOnlyLayer : Layer
    {
        private readonly ILayer _source;

        public VertexOnlyLayer(ILayer source)
        {
            _source = source;
            _source.DataChanged += (sender, args) => OnDataChanged(args);
        }

        private IEnumerable<IFeature> GetFeatures2(MRect box, double resolution)
        {
            return _source.GetFeatures(box, resolution);
        }

        public new void DataHasChanged()
        {
            _source.DataHasChanged();
        }

        public override IEnumerable<IFeature> GetFeatures(MRect box, double resolution)
        {
            var features = GetFeatures2(box, resolution);

            foreach (var feature in features)
            {
                if (feature is PointFeature pf)
                {
                    yield return pf;// new PointFeature(pf);                    
                }
            }
        }
    }
}
