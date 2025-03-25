using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EstacionaFacilAPI.Models
{
    public class Vehicle
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string Plate { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public string OwnerName { get; set; }
        public bool Paid { get; set; }
        public decimal? AmountPaid { get; set; }
        public double? HoursPaid { get; set; }

        public DateTime? EntryTime { get; set; }
        public DateTime? ExitTime { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? AttendedByUserId { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string? ReleasedByUserId { get; set; }
    }
}
