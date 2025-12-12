using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace dotnet.models
{
    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
    public class VerifyOtpModel
    {
        public string Name { get; set; } = string.Empty;
        public string Otp { get; set; } = string.Empty;
    }
    public class SignupModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public required string Id { get; set; } = string.Empty;
        public required string IdNo { get; set; }
        public required string Name { get; set; }
        public required string Password { get; set; }
        public required string plain{ get; set; }
        public required string email { get; set; }
        public required string designation { get; set; }
        public required string department { get; set; }
        public required string rolename { get; set; } = string.Empty;
        public required long mobilenumber { get; set; }
    }
    public class UserModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        public required string IdNo { get; set; }
        public required string Name { get; set; }
        public required string Password { get; set; }
        public required string plain{ get; set; }
    }
    public class UpdateUserModel
    {
        public required string IdNo { get; set; }
        public required string Name { get; set; }
        public required string Password { get; set; }
        public required string plain{ get; set; }
    }
    public class EmailSettings
    {
        public required string FromEmail { get; set; }
        public required string DisplayName { get; set; }
        public required string Password { get; set; }
        public required string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSSL { get; set; }
    }

}
