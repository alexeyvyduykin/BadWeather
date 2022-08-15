using Newtonsoft.Json;

namespace BadWeather.Services.OpenWeather
{
    public class Wind
    {
        /// <summary>
        /// Wind speed. 
        /// Unit Default: meter/sec, Metric: meter/sec, Imperial: miles/hour.
        /// </summary>
        [JsonProperty("speed")]
        public double Speed { get; set; }

        /// <summary>
        /// Wind direction, degrees (meteorological)
        /// </summary>
        [JsonProperty("deg")]
        public int Degree { get; set; }

        /// <summary>
        /// Wind gust. 
        /// Unit Default: meter/sec, Metric: meter/sec, Imperial: miles/hour
        /// </summary>
        [JsonProperty("gust")]
        public double Gust { get; set; }
    }
}
