using Newtonsoft.Json;

namespace BadWeather.Services.OpenWeather
{
    public class Sys
    {
        /// <summary>
        /// Internal parameter
        /// </summary>
        [JsonProperty("type")]
        public int Type { get; set; }

        /// <summary>
        /// Internal parameter
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Internal parameter
        /// </summary>
        [JsonProperty("message")]
        public double Message { get; set; }

        /// <summary>
        /// Country code (GB, JP etc.)
        /// </summary>
        [JsonProperty("country")]
        public string? Country { get; set; }

        /// <summary>
        /// Sunrise time, unix, UTC
        /// </summary>
        [JsonProperty("sunrise")]
        [JsonConverter(typeof(UnixToDateTimeConverter))]
        public DateTime Sunrise { get; set; }

        /// <summary>
        /// Sunset time, unix, UTC
        /// </summary>
        [JsonProperty("sunset")]
        [JsonConverter(typeof(UnixToDateTimeConverter))]
        public DateTime Sunset { get; set; }
    }

    public class UnixToDateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime ReadJson(JsonReader reader, Type objectType, DateTime existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var unixTime = (long?)reader.Value;

            if (unixTime == null)
            {
                throw new NotImplementedException();
            }

            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

            return dateTime.AddSeconds((double)unixTime).ToLocalTime();
        }

        public override void WriteJson(JsonWriter writer, DateTime value, JsonSerializer serializer)
        {
            var dateTimeOffset = new DateTimeOffset(value);

            writer.WriteValue(dateTimeOffset.ToUnixTimeSeconds());
        }
    }
}
