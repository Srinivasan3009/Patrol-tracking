using MongoDB.Driver;
using dotnet.models;
using dotnet.services;
namespace dotnet.services
{
    public class TaskServices
    {
        private readonly IMongoCollection<TaskModel> _task;
        public TaskServices(IMongoDatabase database)
        {
            _task = database.GetCollection<TaskModel>("task");
        }
        public async Task<List<TaskModel>> GetAll()
        {
            return await _task.Find(_ => true).ToListAsync();
        }
        public async Task<bool> Updatetask(TaskModel model)
        {
            var filter = Builders<TaskModel>.Filter.Eq(e => e.Taskid, model.Taskid);
            var update = Builders<TaskModel>.Update.Set(e => e.Taskname, model.Taskname).Set(e => e.Remarks, model.Remarks).Set(e => e.Start, model.Start).Set(e => e.End, model.End);
            await _task.UpdateOneAsync(filter, update);
            return true;
        }
        public async Task<bool> Deletetask(string Taskid)
        {
            var task = Builders<TaskModel>.Filter.Eq(e => e.Taskid, Taskid);
            await _task.DeleteOneAsync(task);
            return true;
        }
        public async Task<string> Addtask(TaskModel model)
        {
            var existing = await _task.Find(e => e.Taskname == model.Taskname).FirstOrDefaultAsync();
            if (existing != null)
            {
                return "Task already exists!";
            }
            var count = await _task.CountDocumentsAsync(FilterDefinition<TaskModel>.Empty);
            var newIdNo = $"{(count + 1).ToString("D3")}";
            while (await _task.Find(e => e.Taskid == newIdNo).AnyAsync())
            {
                count++;
                newIdNo = $"{(count + 1).ToString("D3")}";
            }
            var newtask = new TaskModel
            {
                Id = "",
                Taskid = newIdNo,
                Wfid = model.Wfid,
                Taskname = model.Taskname,
                Remarks = model.Remarks,
                Start = model.Start,
                End = model.End,
                Assignedto = ""
            };
            await _task.InsertOneAsync(newtask);
            return "Success";
        }
         public async Task<bool> AssignTaskAsync(string taskid, string name)
    {
        var filter = Builders<TaskModel>.Filter.Eq(t => t.Taskid, taskid);
        var update = Builders<TaskModel>.Update.Set(t => t.Assignedto, name);

        var result = await _task.UpdateOneAsync(filter, update);
        return result.MatchedCount > 0;
    }
    }
}
