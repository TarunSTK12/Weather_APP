namespace BlazorApp.Server.Models
{
    public class WeatherData
    {
        public string Name { get; set; } = string.Empty;
        public double Temperature { get; set; } // Add this line
        public string Weather { get; set; } = string.Empty; // Add this line
        public double WindSpeed { get; set; } // Add this line
        public int Humidity { get; set; } // Add this line
        public double Pressure { get; set; } // Add this line
        public double Visibility { get; set; } // Add this line
        public string Description { get; set; } = string.Empty; // Add this line
        public long Sunrise { get; set; } // Add this line
        public long Sunset { get; set; } // Add this line
        public string Date { get; set; } = string.Empty; // Add this line

        public MainWeather Main { get; set; } = new();
        public List<WeatherDescription> WeatherList { get; set; } = new(); // Renamed to avoid conflict
        public WindData Wind { get; set; } = new();
    }

    public class MainWeather
    {
        public double Temp { get; set; }
        public double Pressure { get; set; }
        public int Humidity { get; set; }
    }

    public class WeatherDescription
    {
        public string Main { get; set; } = string.Empty; // Add this line
        public string Description { get; set; } = string.Empty;
    }

    public class WindData
    {
        public double Speed { get; set; }
    }
}