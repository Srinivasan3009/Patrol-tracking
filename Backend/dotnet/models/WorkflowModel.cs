using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace dotnet.models
{
    public class WorkflowModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public required string Id { get; set; } = string.Empty;
        public required string Wfid { get; set; }
        public required string Wftitle { get; set; }
        public required string Description { get; set; }
        public required string Status { get; set; } = "Pending";
        public required DateTime Start { get; set; }
        public required DateTime End { get; set; }
        public required string Createdby { get; set; }
        public required string Modifiedby { get; set; }
        public DateTime? Modifieddate { get; set; }
        public required bool Active { get; set; } = false;
    }
public class AssignTaskRequest
    {
        public string Taskname { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty; // Assignedto
    }


}