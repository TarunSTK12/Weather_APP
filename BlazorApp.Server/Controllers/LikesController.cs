using Microsoft.AspNetCore.Mvc;
using BlazorApp.Server.Services;
using BlazorApp.Server.Models;
using MongoDB.Driver;
using System.Text.Json;
namespace BlazorApp.Server.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    //public class LikesController : ControllerBase
    //{
    //    private readonly MongoDBService _mongoDBService;

    //    public LikesController(MongoDBService mongoDBService)
    //    {
    //        _mongoDBService = mongoDBService;
    //    }

    //    [HttpPost]
    //    public async Task<IActionResult> CreateLike([FromBody] Like like)
    //    {
    //        await _mongoDBService.CreateLike(like);
    //        return Ok();
    //    }

    //    [HttpGet("{userId}")]
    //    public async Task<IActionResult> GetLikesByUser(string userId)
    //    {
    //        var likes = await _mongoDBService.GetLikesByUser(userId);
    //        return Ok(likes);
    //    }
    //}
    //[Route("api/likes")]
    [ApiController]
    [Route("api/likes")]
    public class LikesController : ControllerBase

    {
        private readonly IMongoCollection<Like> _likesCollection;

        public LikesController(IMongoDatabase database)
        {
            _likesCollection = database.GetCollection<Like>("Likes");
        }

        [HttpPost]
        public async Task<IActionResult> AddLike([FromBody] Like like)
        {
            try
            {
                if (like == null || string.IsNullOrEmpty(like.UserId))
                    return BadRequest("Invalid like data.");

                like.Timestamp = DateTime.UtcNow;
                like.Liked = 1;

                await _likesCollection.InsertOneAsync(like);

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding like: {ex.Message}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("liked-weather/{userId}")]
        public async Task<IActionResult> GetLikedWeather(string userId, [FromServices] IHttpClientFactory httpClientFactory)
        {
            try
            {
                // Filter for documents where userId matches and Liked == 1
                var filter = Builders<Like>.Filter.Eq(l => l.UserId, userId) &
                             Builders<Like>.Filter.Eq(l => l.Liked, 1);

                // Get distinct liked city names
                var likedCities = await _likesCollection.Find(filter)
                                                        .Project(l => l.Location)
                                                        .ToListAsync();

                if (!likedCities.Any())
                {
                    return Ok(new List<WeatherData>()); // Return empty list if no liked cities
                }

                var httpClient = httpClientFactory.CreateClient("WeatherAPI");
                var weatherResults = new List<WeatherData>();

                foreach (var city in likedCities.Distinct())
                {
                    var response = await httpClient.GetAsync($"weather?city={Uri.EscapeDataString(city)}");
                    if (response.IsSuccessStatusCode)
                    {
                        var weatherJson = await response.Content.ReadAsStringAsync();
                        var weather = JsonSerializer.Deserialize<WeatherData>(weatherJson, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        if (weather != null)
                        {
                            weatherResults.Add(weather);
                        }
                    }
                }

                return Ok(weatherResults);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching liked weather: {ex.Message}");
                return StatusCode(500, "Unable to get liked cities weather.");
            }
        }


        public class Like
        {
            public string UserId { get; set; } = string.Empty;
            public string UserEmail { get; set; } = string.Empty;
            public string Location { get; set; } = string.Empty;
            public int Liked { get; set; } = 1;
            public DateTime Timestamp { get; set; }
        }

    }
}