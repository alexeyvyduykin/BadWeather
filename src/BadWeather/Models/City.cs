using BadWeather.Services.OpenWeather;
using Newtonsoft.Json;

namespace BadWeather.Models
{
    public class City
    {
        [JsonProperty("city")]
        public string? Name { get; set; }

        [JsonProperty("lng")]
        public double Lon { get; set; }

        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("country")]
        public string? Country { get; set; }

        [JsonProperty("iso2")]
        public string? Code { get; set; }

        [JsonProperty("admin_name")]
        public string? AdminName { get; set; }

        [JsonProperty("capital")]
        public string? Capital { get; set; }

        [JsonProperty("population")]
        public int? Population { get; set; }

        [JsonProperty("population_proper")]
        public int? PopulationProper { get; set; }
    }

    public class OpenWeatherCity
    {
        [JsonProperty("id")]
        public double Id { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("state")]
        public string? State { get; set; }

        [JsonProperty("country")]
        public string? Country { get; set; }

        [JsonProperty("coord")]
        public Coordinate? Coordinate { get; set; }
    }
}
