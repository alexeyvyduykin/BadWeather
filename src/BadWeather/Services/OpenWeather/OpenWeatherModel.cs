using Newtonsoft.Json;

namespace BadWeather.Services.OpenWeather
{
    public class OpenWeatherModel
    {
        [JsonProperty("coord")]
        public Coordinate? Coordinate { get; set; }

        [JsonProperty("weather")]
        public List<Weather>? Weathers { get; set; }

        /// <summary>
        /// Internal parameter 
        /// </summary>
        [JsonProperty("base")]
        public string? Base { get; set; }

        [JsonProperty("main")]
        public Main? Main { get; set; }

        /// <summary>
        /// Visibility, meter. The maximum value of the visibility is 10km
        /// </summary>
        [JsonProperty("visibility")]
        public int Visibility { get; set; }

        [JsonProperty("wind")]
        public Wind? Wind { get; set; }

        [JsonProperty("clouds")]
        public Clouds? Clouds { get; set; }

        [JsonProperty("rain")]
        public Rain? Rain { get; set; }

        [JsonProperty("snow")]
        public Snow? Snow { get; set; }

        /// <summary>
        /// Time of data calculation, unix, UTC 
        /// </summary>
        [JsonProperty("dt")]
        public int DataTime { get; set; }

        [JsonProperty("sys")]
        public Sys? Sys { get; set; }

        /// <summary>
        /// Shift in seconds from UTC 
        /// </summary>
        [JsonProperty("timezone")]
        public int Timezone { get; set; }

        /// <summary>
        /// City ID
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// City name 
        /// </summary>
        [JsonProperty("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Internal parameter
        /// </summary>
        [JsonProperty("cod")]
        public int Cod { get; set; }
    }
}
