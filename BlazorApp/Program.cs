using BlazorApp;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using BlazorApp.Services;
using Supabase;
//using Supabase.Gotrue;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Register HttpClient
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5227") });

// Register WeatherService
builder.Services.AddScoped<GetWeatherService>();

builder.Services.AddScoped(provider =>
    new Supabase.Client(
        "https://tkoqcfvtytjcmqhlrrun.supabase.co",
        "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InRrb3FjZnZ0eXRqY21xaGxycnVuIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDI2MjQ2MzQsImV4cCI6MjA1ODIwMDYzNH0.BYHcVcPLnPq75SjOAqDhoDVfHjA_r3x-p_h28S6ct3E",
        new Supabase.SupabaseOptions
        {
            AutoRefreshToken = true,
            AutoConnectRealtime = true
        }
    )
);

// Register AuthService
builder.Services.AddScoped<AuthService>();

await builder.Build().RunAsync();