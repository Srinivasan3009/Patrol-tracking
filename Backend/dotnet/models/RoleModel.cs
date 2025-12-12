using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace dotnet.models
{
    public class RoleModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public required string Id { get; set; } = string.Empty;
        public required string RoleId { get; set; }
        public required string RoleName { get; set; }
        public required string Description { get; set; }
    }
}