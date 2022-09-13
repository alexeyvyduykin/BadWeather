using BadWeather.Services.OpenWeather;
using ReactiveUI;

namespace BadWeather.ViewModels
{
    public class CityViewModel : ReactiveObject
    {
        public CityViewModel(OpenWeatherModel model)
        {
            Name = model.Name!;
            Lon = model.Coordinate!.Lon;
            Lat = model.Coordinate!.Lat;
            Temperature = ToCelcius(model.Main!.Temperature);
            TemperatureFeelsLike = ToCelcius(model.Main.TemperatureFeelsLike);
            Pressure = model.Main.Pressure;
            Humidity = model.Main.Humidity;
            Icon = model.Weathers!.First().Icon!;
            DataCalculation = model.DataTime;
            Speed = model.Wind!.Speed;
            Degree = model.Wind!.Degree;
            Gust = model.Wind!.Gust;
            Cloudiness = model.Clouds!.All;
        }

        protected double ToCelcius(double value) => Math.Round(value - 273.15, 3);

        public string Name { get; set; }

        public double Lon { get; set; }

        public double Lat { get; set; }

        public double Temperature { get; set; }

        public double TemperatureFeelsLike { get; set; }

        public double Pressure { get; set; }

        public int Humidity { get; set; }

        public string Icon { get; set; }

        public int DataCalculation { get; set; }

        public double Speed { get; set; }

        public int Degree { get; set; }

        public double Gust { get; set; }

        public int Cloudiness { get; set; }
    }
}
