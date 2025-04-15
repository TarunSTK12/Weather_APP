namespace BlazorApp.Server.Models
{
    public class WeatherData
    {
        public string Name { get; set; } = string.Empty; // City name
        public MainWeather Main { get; set; } = new(); // Holds temperature, pressure, humidity
        public List<WeatherDescription> Weather { get; set; } = new(); // Fixed to match API response
        public WindData Wind { get; set; } = new(); // Fixed wind data mapping
        public int Visibility { get; set; } // Fixed data type
        public SysData Sys { get; set; } = new(); // Holds sunrise & sunset data
        public string Date { get; set; } = string.Empty; // Can be manually formatted
    }

    public class MainWeather
    {
        public double Temp { get; set; }
        public double Pressure { get; set; }
        public int Humidity { get; set; }
    }

    public class WeatherDescription
    {
        public string Main { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class WindData
    {
        public double Speed { get; set; }
    }

    public class SysData
    {
        public long Sunrise { get; set; }
        public long Sunset { get; set; }
    }
}
