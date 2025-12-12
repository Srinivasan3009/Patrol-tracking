using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace dotnet.models
{
    public class TaskModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public required string Id { get; set; } = string.Empty;
        public required string Taskid { get; set; }
        public required string Taskname { get; set; }
        public required string Remarks { get; set; }
        public required string Wfid { get; set; }
        public required DateTime Start { get; set; }
        public required DateTime End { get; set; }
        public required string Assignedto{ get; set; }
    }
}