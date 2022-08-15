using Newtonsoft.Json;

namespace BadWeather.Services.OpenWeather
{
    public class Weather
    {
        /// <summary>
        ///  Weather condition id
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Group of weather parameters (Rain, Snow, Extreme etc.)
        /// </summary>
        [JsonProperty("main")]
        public string? Main { get; set; }

        /// <summary>
        /// Weather condition within the group. You can get the output in your language
        /// </summary>
        [JsonProperty("description")]
        public string? Description { get; set; }

        /// <summary>
        /// Weather icon id
        /// </summary>
        [JsonProperty("icon")]
        public string? Icon { get; set; }
    }
}
