using MongoDB.Driver;
using dotnet.services;
using dotnet.models;
namespace dotnet.services
{
    public class RoleServices
    {
        private readonly IMongoCollection<RoleModel> _role;
        public RoleServices(IMongoDatabase database)
        {
            _role = database.GetCollection<RoleModel>("role");
        }
        public async Task<List<RoleModel>> GetAll()
        {
            return await _role.Find(_ => true).ToListAsync();
        }
        public async Task<bool> UpdateRole(RoleModel model)
        {
            var filter = Builders<RoleModel>.Filter.Eq(e => e.RoleId, model.RoleId);
            var update = Builders<RoleModel>.Update.Set(e => e.RoleName, model.RoleName).Set(e => e.Description, model.Description);
            await _role.UpdateOneAsync(filter, update);
            return true;
        }
        public async Task<bool> DeleteRole(string id)
        {
            var role = Builders<RoleModel>.Filter.Eq(e => e.RoleId, id);
            await _role.DeleteOneAsync(role);
            return true;
        }
        public async Task<string> AddRole(RoleModel model)
        {
            var existing = await _role.Find(e => e.RoleName == model.RoleName).FirstOrDefaultAsync();
            if (existing != null)
            {
                return "Role already exists!";
            }
            var count = await _role.CountDocumentsAsync(FilterDefinition<RoleModel>.Empty);
            var newIdNo = $"{(count + 1).ToString("D3")}";
            while (await _role.Find(e => e.RoleId == newIdNo).AnyAsync())
            {
                count++;
                newIdNo = $"{(count + 1).ToString("D3")}";
            }
            var newrole = new RoleModel
            {
                Id= "",
                RoleId = newIdNo,
                RoleName = model.RoleName,
                Description = model.Description
            };
            await _role.InsertOneAsync(newrole);
            return "Success";
        }
    }
}