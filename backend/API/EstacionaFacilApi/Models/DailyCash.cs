using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EstacionaFacilAPI.Models
{
    public class CashTransaction
    {
        public string VehiclePlate { get; set; }
        public decimal Amount { get; set; }
        public string AttendedByUserId { get; set; }
        public string AttendedByUserName { get; set; }
        public string Type { get; set; } // "entry" ou "exit"
        public DateTime Timestamp { get; set; }
    }

    public class DailyCash
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public DateTime Date { get; set; }
        public decimal Total { get; set; } = 0;

        public List<CashTransaction> Transactions { get; set; } = new();
    }
}
