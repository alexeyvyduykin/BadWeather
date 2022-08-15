using Newtonsoft.Json;

namespace BadWeather.Services.OpenWeather
{
    public class Clouds
    {
        /// <summary>
        /// Cloudiness, %
        /// </summary>
        [JsonProperty("all")]
        public int All { get; set; }
    }
}
