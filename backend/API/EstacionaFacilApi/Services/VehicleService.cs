using EstacionaFacilAPI.Data;
using EstacionaFacilAPI.Models;
using MongoDB.Driver;

namespace EstacionaFacilAPI.Services
{
    public class VehicleService
    {
        private readonly IMongoCollection<Vehicle> _vehicles;

        public VehicleService(MongoDBContext context)
        {
            _vehicles = context.Vehicles;
        }

        public async Task<List<Vehicle>> GetAllAsync()
        {
            return await _vehicles.Find(v => true).ToListAsync();
        }

        public async Task<Vehicle> RegisterEntryAsync(Vehicle vehicle)
        {
            vehicle.EntryTime = DateTime.Now;
            vehicle.ExitTime = null; // ainda está no pátio
            await _vehicles.InsertOneAsync(vehicle);
            return vehicle;
        }

        public async Task<Vehicle?> RegisterExitAsync(string id, string userId)
        {
            var filter = Builders<Vehicle>.Filter.Eq(v => v.Id, id);
            var update = Builders<Vehicle>.Update
                .Set(v => v.ExitTime, DateTime.Now)
                .Set(v => v.ReleasedByUserId, userId);

            var result = await _vehicles.UpdateOneAsync(filter, update);

            if (result.ModifiedCount == 0)
                return null;

            return await _vehicles.Find(v => v.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Vehicle>> GetActiveVehiclesAsync()
        {
            var filter = Builders<Vehicle>.Filter.Eq(v => v.ExitTime, null);
            return await _vehicles.Find(filter).ToListAsync();
        }
    }
}
