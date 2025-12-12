using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace dotnet.models
{
    public class LocationModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? LocationId { get; set; }

        [BsonElement("locationCode")]
        public string LocationCode { get; set; } = string.Empty;

        [BsonElement("locationName")]
        public string LocationName { get; set; } = string.Empty;

        [BsonElement("description")]
        public string Description { get; set; } = string.Empty;

        [BsonElement("coordinates")]
        public Coordinates Coordinates { get; set; } = new Coordinates();
    }

    public class Coordinates
    {
        [BsonElement("latitude")]
        public double Latitude { get; set; }

        [BsonElement("longitude")]
        public double Longitude { get; set; }
    }
}
