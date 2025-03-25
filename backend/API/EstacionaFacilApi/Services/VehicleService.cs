using EstacionaFacilAPI.Data;
using EstacionaFacilAPI.Models;
using MongoDB.Driver;

namespace EstacionaFacilAPI.Services
{
    public class VehicleService
    {
        private readonly IMongoCollection<Vehicle> _vehicles;
        private readonly IMongoCollection<User> _users;

        public VehicleService(MongoDBContext context)
        {
            _vehicles = context.Vehicles;
            _users = context.Users;
        }

        public async Task<List<Vehicle>> GetAllAsync()
        {
            return await _vehicles.Find(v => true).ToListAsync();
        }

        public async Task<Vehicle> RegisterEntryAsync(Vehicle vehicle, DailyCashService cashService, string userName)
        {
            vehicle.EntryTime = DateTime.Now;
            vehicle.ExitTime = null;

            await _vehicles.InsertOneAsync(vehicle);

            // Registrar no caixa se já foi pago na entrada
            if (vehicle.Paid && vehicle.AmountPaid != null && vehicle.AmountPaid > 0)
            {
                await cashService.AddToCashAsync(
                    vehicle.AmountPaid.Value,
                    vehicle.Plate,
                    vehicle.AttendedByUserId,
                    userName,
                    "entry"
                );
            }

            return vehicle;
        }

        public async Task<Vehicle?> RegisterExitAsync(string id, string userId, decimal? additionalAmount, DailyCashService cashService)
        {
            var vehicle = await _vehicles.Find(v => v.Id == id).FirstOrDefaultAsync();
            if (vehicle == null || vehicle.ExitTime != null)
                return null;

            var totalHoras = (DateTime.Now - vehicle.EntryTime)?.TotalHours ?? 0;
            var horasPagas = vehicle.HoursPaid ?? 0;

            if (vehicle.Paid && totalHoras > horasPagas && additionalAmount == null)
            {
                throw new Exception("Tempo excedido. Valor adicional deve ser informado.");
            }

            var user = await _users.Find(u => u.Id == userId).FirstOrDefaultAsync();
            var userName = user?.Name ?? "Funcionário";

            // Registrar o pagamento no caixa
            if (additionalAmount != null && additionalAmount > 0)
            {
                vehicle.AmountPaid = (vehicle.AmountPaid ?? 0) + additionalAmount.Value;
                await cashService.AddToCashAsync(additionalAmount.Value, vehicle.Plate, userId, userName, "exit");
            }
            else if (vehicle.AmountPaid != null)
            {
                await cashService.AddToCashAsync(vehicle.AmountPaid.Value, vehicle.Plate, userId, userName, "exit");
            }

            vehicle.ExitTime = DateTime.Now;
            vehicle.ReleasedByUserId = userId;

            await _vehicles.ReplaceOneAsync(v => v.Id == id, vehicle);
            return vehicle;
        }

        public async Task<List<Vehicle>> GetActiveVehiclesAsync()
        {
            var filter = Builders<Vehicle>.Filter.Eq(v => v.ExitTime, null);
            return await _vehicles.Find(filter).ToListAsync();
        }

        public async Task<List<Vehicle>> GetAllVehiclesAsync()
        {
            return await _vehicles.Find(_ => true).ToListAsync();
        }
    }
}
