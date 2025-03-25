using EstacionaFacilAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace EstacionaFacilAPI.Data
{
    public class MongoDBContext
    {
        private readonly IMongoDatabase _database;

        public MongoDBContext(IOptions<MongoDBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<DailyCash> DailyCash => _database.GetCollection<DailyCash>("DailyCash");
        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
        public IMongoCollection<Vehicle> Vehicles => _database.GetCollection<Vehicle>("Vehicles");
    }

    public class MongoDBSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
