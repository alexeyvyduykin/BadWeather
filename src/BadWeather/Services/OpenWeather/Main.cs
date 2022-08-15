using Newtonsoft.Json;

namespace BadWeather.Services.OpenWeather
{
    public class Main
    {
        private double? _temperatureInCelsius;

        /// <summary>
        /// Temperature. 
        /// Unit Default: Kelvin, Metric: Celsius, Imperial: Fahrenheit. 
        /// </summary>
        [JsonProperty("temp")]
        public double Temperature { get; set; }

        /// <summary>
        /// Temperature. This temperature parameter accounts for the human perception of weather. 
        /// Unit Default: Kelvin, Metric: Celsius, Imperial: Fahrenheit. 
        /// </summary>
        [JsonProperty("feels_like")]
        public double TemperatureFeelsLike { get; set; }

        /// <summary>
        /// Minimum temperature at the moment. This is minimal currently observed temperature (within large megalopolises and urban areas). 
        /// Unit Default: Kelvin, Metric: Celsius, Imperial: Fahrenheit.
        /// </summary>
        [JsonProperty("temp_min")]
        public double MinTemperature { get; set; }

        /// <summary>
        /// Maximum temperature at the moment. This is maximal currently observed temperature (within large megalopolises and urban areas).
        /// Unit Default: Kelvin, Metric: Celsius, Imperial: Fahrenheit.
        /// </summary>
        [JsonProperty("temp_max")]
        public double MaxTemperature { get; set; }

        /// <summary>
        /// Atmospheric pressure (on the sea level, if there is no sea_level or grnd_level data), hPa
        /// </summary>
        [JsonProperty("pressure")]
        public double Pressure { get; set; }

        /// <summary>
        /// Humidity, %
        /// </summary>
        [JsonProperty("humidity")]
        public int Humidity { get; set; }

        public double GetTemperatureInCelsius() => _temperatureInCelsius ??= ConvertToCelsius(Temperature);

        private static double ConvertToCelsius(double kelvin)
        {
            return Math.Round(kelvin - 273.15, 3);
        }
    }
}
