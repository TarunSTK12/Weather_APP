using MongoDB.Driver;
using BlazorApp.Server.Models;

namespace BlazorApp.Server.Services
{
    public class MongoDBSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
    }
}