using BlazorApp.Server.Services;
using MongoDB.Driver;

//using BlazorApp;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDB"));

builder.Services.AddSingleton<IMongoClient>(sp =>
    new MongoClient(builder.Configuration["MongoDB:ConnectionString"]));

builder.Services.AddScoped<IMongoDatabase>(sp =>
    sp.GetRequiredService<IMongoClient>().GetDatabase("WeatherForecast"));


// Add CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        policy =>
        {
            policy.AllowAnyOrigin()   // Allows requests from any domain
                  .AllowAnyMethod()   // Allows GET, POST, PUT, DELETE, etc.
                  .AllowAnyHeader();  // Allows all headers
        });
});



// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register HttpClient and WeatherForecastService
builder.Services.AddHttpClient<WeatherForecastService>();
//builder.Services.AddScoped<AuthService>();

builder.Services.AddHttpClient("WeatherAPI", client =>
{
    client.BaseAddress = new Uri("http://localhost:5291/"); // Must point to your actual weather API controller
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Apply CORS Policy
app.UseCors("AllowAllOrigins");


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
