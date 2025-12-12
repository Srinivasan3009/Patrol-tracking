using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Microsoft.AspNetCore.SignalR;
namespace dotnet.models
{
    public class employeeModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public required string Id { get; set; } = string.Empty;
        public required string IdNo { get; set; }
        public required string Name { get; set; }
        public required string Password { get; set; }
        public required string plain{ get; set; }
        public required string email { get; set; }
        public required string designation{ get; set; }
        public required string department{ get; set; }
        public required string rolename { get; set; } = string.Empty;
        public required long mobilenumber{ get; set; }
        public string? Otp { get; set; }
        public DateTime? OtpGeneratedAt { get; set; }
        
    }
 }