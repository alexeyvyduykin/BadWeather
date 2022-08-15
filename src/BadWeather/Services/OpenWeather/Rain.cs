using Newtonsoft.Json;

namespace BadWeather.Services.OpenWeather
{
    public class Rain
    {
        /// <summary>
        /// Rain volume for the last 1 hour, mm
        /// </summary>
        [JsonProperty("1h")]
        public double Volume1h { get; set; }

        /// <summary>
        /// Rain volume for the last 3 hours, mm
        /// </summary>
        [JsonProperty("3h")]
        public double Volume3h { get; set; }
    }
}
