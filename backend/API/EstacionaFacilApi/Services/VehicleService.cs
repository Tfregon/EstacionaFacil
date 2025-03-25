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

        public async Task<Vehicle> RegisterExitAsync(string id)
        {
            var vehicle = await _vehicles.Find(v => v.Id == id).FirstOrDefaultAsync();
            if (vehicle == null || vehicle.ExitTime != null)
                return null;

            vehicle.ExitTime = DateTime.Now;
            await _vehicles.ReplaceOneAsync(v => v.Id == id, vehicle);
            return vehicle;
        }
    }
}
