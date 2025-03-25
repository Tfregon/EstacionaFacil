using EstacionaFacilAPI.Data;
using EstacionaFacilAPI.Models;
using MongoDB.Driver;

namespace EstacionaFacilAPI.Services
{
    public class DailyCashService
    {
        private readonly IMongoCollection<DailyCash> _cash;

        public DailyCashService(MongoDBContext context)
        {
            _cash = context.DailyCash;
        }

        public async Task AddToCashAsync(decimal amount, string plate, string userId, string userName, string type)
        {
            var today = DateTime.Today;
            var filter = Builders<DailyCash>.Filter.Eq(c => c.Date, today);

            var update = Builders<DailyCash>.Update
                .Inc(c => c.Total, amount)
                .Push(c => c.Transactions, new CashTransaction
                {
                    VehiclePlate = plate,
                    Amount = amount,
                    AttendedByUserId = userId,
                    AttendedByUserName = userName,
                    Type = type,
                    Timestamp = DateTime.Now
                });

            var options = new UpdateOptions { IsUpsert = true };

            await _cash.UpdateOneAsync(filter, update, options);
        }


        public async Task<DailyCash?> GetByDateAsync(DateTime date)
        {
            var target = date.Date;
            return await _cash.Find(c => c.Date == target).FirstOrDefaultAsync();
        }

        public async Task<DailyCash?> GetTodayAsync()
        {
            return await GetByDateAsync(DateTime.Today);
        }
    }
}
