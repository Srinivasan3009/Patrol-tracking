using MongoDB.Driver;
using dotnet.models;
using dotnet.services;
namespace dotnet.services
{
    public class WorkflowServices
    {
        private readonly IMongoCollection<WorkflowModel> _wf;
        public WorkflowServices(IMongoDatabase database)
        {
            _wf = database.GetCollection<WorkflowModel>("workflow");
        }
        public async Task<List<WorkflowModel>> GetAll()
        {
            return await _wf.Find(_ => true).ToListAsync();
        }
        public async Task<bool> UpdateWf(WorkflowModel model)
        {
            var filter = Builders<WorkflowModel>.Filter.Eq(e => e.Wfid, model.Wfid);
            var update = Builders<WorkflowModel>.Update.Set(e => e.Wfid, model.Wfid).Set(e => e.Wftitle, model.Wftitle).Set(e => e.Description, model.Description).Set(e => e.Start, model.Start).Set(e => e.End, model.End).Set(e => e.Active, model.Active).Set(e => e.Modifiedby, model.Modifiedby).Set(e => e.Modifieddate, DateTime.UtcNow);
            await _wf.UpdateOneAsync(filter, update);
            return true;
        }
        public async Task<bool> DeleteWf(string Wfid)
        {
            var wf = Builders<WorkflowModel>.Filter.Eq(e => e.Wfid, Wfid);
            await _wf.DeleteOneAsync(wf);
            return true;
        }
        public async Task<string> AddWf(WorkflowModel model)
        {
            var existing = await _wf.Find(e => e.Wftitle == model.Wftitle).FirstOrDefaultAsync();
            if (existing != null)
            {
                return "Role already exists!";
            }
            var count = await _wf.CountDocumentsAsync(FilterDefinition<WorkflowModel>.Empty);
            var newIdNo = $"{(count + 1).ToString("D3")}";
            while (await _wf.Find(e => e.Wfid == newIdNo).AnyAsync())
            {
                count++;
                newIdNo = $"{(count + 1).ToString("D3")}";
            }
            var newwf = new WorkflowModel
            {
                Id = "",
                Wfid = newIdNo,
                Wftitle = model.Wftitle,
                Description = model.Description,
                Status = model.Status,
                Start = model.Start,
                End = model.End,
                Createdby = model.Createdby,
                Modifiedby = "",
                Modifieddate = null ,
                Active = model.Active
            };
            await _wf.InsertOneAsync(newwf);
            return "Success";
        }
    }
}
