using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using WeatherIcons.Avalonia.Enums;

namespace BadWeather.Avalonia.Converters
{
    public class WeatherKeyConverter : IValueConverter
    {
        private static readonly IDictionary<string, ThemedWeatherKey> _dict = new Dictionary<string, ThemedWeatherKey>()
        {
            { "01d", ThemedWeatherKey.DaySunny },
            { "01n", ThemedWeatherKey.NightClear },
            { "02d", ThemedWeatherKey.DayCloudy },
            { "02n", ThemedWeatherKey.NightCloudy },
            { "03d", ThemedWeatherKey.Cloud },
            { "03n", ThemedWeatherKey.Cloud },
            { "04d", ThemedWeatherKey.Cloudy },
            { "04n", ThemedWeatherKey.Cloudy },
            { "09d", ThemedWeatherKey.Showers },
            { "09n", ThemedWeatherKey.Showers },
            { "10d", ThemedWeatherKey.DayShowers },
            { "10n", ThemedWeatherKey.NightShowers },
            { "11d", ThemedWeatherKey.DayLightning },
            { "11n", ThemedWeatherKey.NightLightning },
            { "13d", ThemedWeatherKey.DaySnow },
            { "13n", ThemedWeatherKey.NightSnow },
            { "50d", ThemedWeatherKey.DayFog },
            { "50n", ThemedWeatherKey.NightFog },
        };

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string str)
            {
                if (_dict.TryGetValue(str, out var res) == true)
                {
                    return res;
                }
            }

            return ThemedWeatherKey.DaySunny;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
