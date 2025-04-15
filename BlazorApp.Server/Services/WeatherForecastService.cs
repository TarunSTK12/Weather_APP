using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using BlazorApp.Server.Models;
using System.Text.Json;

namespace BlazorApp.Server.Services
{
    public class WeatherForecastService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly string _apiKey;

        public WeatherForecastService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["WeatherAPI:BaseUrl"] ?? "";
            _apiKey = configuration["WeatherAPI:ApiKey"] ?? "";
            Console.WriteLine($"API Base URL: {_baseUrl}");
            Console.WriteLine($"API Key: {_apiKey}");
        }

        public async Task<WeatherData?> GetWeatherAsync(string city)
        {
            if (string.IsNullOrEmpty(city)) return null;

            var url = $"{_baseUrl}?q={city}&appid={_apiKey}&units=metric";

            // Fetch raw JSON response
            var response = await _httpClient.GetStringAsync(url);
            Console.WriteLine($"API Response: {response}"); // Debugging

            // Deserialize manually
            var weatherData = JsonSerializer.Deserialize<WeatherData>(response, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return weatherData;
        }


    }
}
