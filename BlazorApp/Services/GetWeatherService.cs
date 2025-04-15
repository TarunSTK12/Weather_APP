using System.Net.Http.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp.Services
{
    public class GetWeatherService
    {
        private readonly HttpClient _httpClient;
        private const string ApiKey = "8f4b079285f1cca267125c719880c52b";

        public GetWeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Method to fetch current weather by city name
        public async Task<WeatherData?> GetWeatherAsync(string city)
        {
            var response = await _httpClient.GetFromJsonAsync<GetApiResponse>(
                $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={ApiKey}&units=metric"
            );

            if (response == null) return null;

            return new WeatherData
            {
                Name = response.Name,
                Temperature = response.Main.Temp,
                Weather = response.Weather[0].Main,
                Description = response.Weather[0].Description,
                Wind = response.Wind.Speed,
                Humidity = response.Main.Humidity,
                Pressure = response.Main.Pressure,
                Visibility = response.Visibility / 1000.0, // Convert meters to kilometers
                Sunrise = response.Sys.Sunrise,
                Sunset = response.Sys.Sunset
            };
        }

        // Method to fetch current weather by coordinates
        public async Task<WeatherData?> GetWeatherByCoordsAsync(double lat, double lon)
        {
            var response = await _httpClient.GetFromJsonAsync<GetApiResponse>(
                $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={ApiKey}&units=metric"
            );

            if (response == null) return null;

            return new WeatherData
            {
                Name = response.Name,
                Temperature = response.Main.Temp,
                Weather = response.Weather[0].Main,
                Description = response.Weather[0].Description,
                Wind = response.Wind.Speed,
                Humidity = response.Main.Humidity,
                Pressure = response.Main.Pressure,
                Visibility = response.Visibility / 1000.0, // Convert meters to kilometers
                Sunrise = response.Sys.Sunrise,
                Sunset = response.Sys.Sunset
            };
        }

        // Method to fetch 5-day forecast by city name
        public async Task<List<WeatherData>> Get5DayForecastAsync(string city)
        {
            var response = await _httpClient.GetFromJsonAsync<Get5DayApiResponse>(
                $"https://api.openweathermap.org/data/2.5/forecast?q={city}&appid={ApiKey}&units=metric"
            );

            if (response == null) return new List<WeatherData>();

            return response.List.Select(forecast => new WeatherData
            {
                Name = response.City.Name,
                Temperature = forecast.Main.Temp,
                Weather = forecast.Weather[0].Main,
                Description = forecast.Weather[0].Description,
                Wind = forecast.Wind.Speed,
                Humidity = forecast.Main.Humidity,
                Pressure = forecast.Main.Pressure,
                Visibility = forecast.Visibility / 1000.0, // Convert meters to kilometers
                Sunrise = response.City.Sunrise,
                Sunset = response.City.Sunset,
                Date = forecast.DtTxt // Add this line
            }).ToList();
        }

        // Method to fetch 5-day forecast by coordinates
        public async Task<List<WeatherData>> Get5DayForecastByCoordsAsync(double lat, double lon)
        {
            var response = await _httpClient.GetFromJsonAsync<Get5DayApiResponse>(
                $"https://api.openweathermap.org/data/2.5/forecast?lat={lat}&lon={lon}&appid={ApiKey}&units=metric"
            );

            if (response == null) return new List<WeatherData>();

            return response.List.Select(forecast => new WeatherData
            {
                Name = response.City.Name,
                Temperature = forecast.Main.Temp,
                Weather = forecast.Weather[0].Main,
                Description = forecast.Weather[0].Description,
                Wind = forecast.Wind.Speed,
                Humidity = forecast.Main.Humidity,
                Pressure = forecast.Main.Pressure,
                Visibility = forecast.Visibility / 1000.0, // Convert meters to kilometers
                Sunrise = response.City.Sunrise,
                Sunset = response.City.Sunset,
                Date = forecast.DtTxt // Add this line
            }).ToList();
        }

        // API response classes
        public class GetApiResponse
        {
            public string Name { get; set; } = string.Empty;
            public MainInfo Main { get; set; } = new();
            public List<WeatherCondition> Weather { get; set; } = new();
            public WindInfo Wind { get; set; } = new();
            public int Visibility { get; set; }
            public SysInfo Sys { get; set; } = new();
        }

        public class Get5DayApiResponse
        {
            public CityInfo City { get; set; } = new();
            public List<ForecastInfo> List { get; set; } = new();
        }

        public class CityInfo
        {
            public string Name { get; set; } = string.Empty;
            public long Sunrise { get; set; }
            public long Sunset { get; set; }
        }

        public class ForecastInfo
        {
            public MainInfo Main { get; set; } = new();
            public List<WeatherCondition> Weather { get; set; } = new();
            public WindInfo Wind { get; set; } = new();
            public int Visibility { get; set; }
            public string DtTxt { get; set; } = string.Empty;
        }

        public class MainInfo
        {
            public double Temp { get; set; }
            public double Pressure { get; set; }
            public int Humidity { get; set; }
        }

        public class WeatherCondition
        {
            public string Main { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
        }

        public class WindInfo
        {
            public double Speed { get; set; }
        }

        public class SysInfo
        {
            public long Sunrise { get; set; }
            public long Sunset { get; set; }
        }
    }
}