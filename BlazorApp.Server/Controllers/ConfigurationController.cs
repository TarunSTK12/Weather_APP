using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BlazorApp.Server.Controllers
{
    [Route("api/configuration")]
    [ApiController]
    //[Route("api/[controller]")]
    public class ConfigurationController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ConfigurationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult GetConfiguration()
        {
            var config = new
            {
                SupabaseUrl = _configuration["Supabase:Url"],
                SupabaseKey = _configuration["Supabase:Key"]
            };

            return Ok(config);
        }
    }
}