using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using System.Text.Json;

namespace BlazorApp.Services
{
    public class LocalStorageServices
    {
        private const string LikedCitiesKey = "likedCities";

        public static async Task SaveLikedCitiesAsync(IJSRuntime js, List<string> cities)
        {
            var json = JsonSerializer.Serialize(cities);
            await js.InvokeVoidAsync("localStorage.setItem", LikedCitiesKey, json);
        }

        public static async Task<List<string>> GetLikedCitiesAsync(IJSRuntime js)
        {
            var json = await js.InvokeAsync<string>("localStorage.getItem", LikedCitiesKey);
            return string.IsNullOrEmpty(json) ? new List<string>() : JsonSerializer.Deserialize<List<string>>(json)!;
        }

        // For static use in components without JSRuntime injection (like LikedCities.razor OnInitialized)
        public static bool TryGetLikedCities(out List<string> cities)
        {
            try
            {
                var json = WebAssemblyHostBuilder
                    .CreateDefault()
                    .Services
                    .BuildServiceProvider()
                    .GetRequiredService<IJSRuntime>()
                    .InvokeAsync<string>("localStorage.getItem", LikedCitiesKey)
                    .GetAwaiter()
                    .GetResult();

                cities = string.IsNullOrEmpty(json) ? new List<string>() : JsonSerializer.Deserialize<List<string>>(json)!;
                return true;
            }
            catch
            {
                cities = new List<string>();
                return false;
            }
        }

        public static void SaveLikedCities(List<string> cities)
        {
            var json = JsonSerializer.Serialize(cities);
            WebAssemblyHostBuilder
                .CreateDefault()
                .Services
                .BuildServiceProvider()
                .GetRequiredService<IJSRuntime>()
                .InvokeVoidAsync("localStorage.setItem", LikedCitiesKey, json);
        }
    }
}
