using Newtonsoft.Json;

namespace BadWeather.Services.OpenWeather
{
    public class Coordinate
    {
        /// <summary>
        /// City geo location, longitude
        /// </summary>
        [JsonProperty("lon")]
        public double Lon { get; set; }

        /// <summary>
        /// City geo location, latitude
        /// </summary>
        [JsonProperty("lat")]
        public double Lat { get; set; }
    }
}
