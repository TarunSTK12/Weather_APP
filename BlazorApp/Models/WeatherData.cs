namespace BlazorApp.Services
{
    public class WeatherData
    {
        public string Name { get; set; } = string.Empty;
        public double Temperature { get; set; }
        public string Weather { get; set; } = string.Empty;
        public double Wind { get; set; }
        public int Humidity { get; set; }
        public double Pressure { get; set; }
        public double Visibility { get; set; }
        public string Description { get; set; } = string.Empty;
        public long Sunrise { get; set; }
        public long Sunset { get; set; }
        public string Date { get; set; } = string.Empty; // Add this line

    }
}
