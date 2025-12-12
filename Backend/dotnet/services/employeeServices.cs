using MongoDB.Driver;
using dotnet.models;
namespace dotnet.services
{
    public class employeeServices
    {
        private readonly IMongoCollection<employeeModel> _emp;
        public employeeServices(IMongoDatabase database)
        {
            _emp = database.GetCollection<employeeModel>("employee");
        }
        public async Task<List<employeeModel>> GetAll()
        {
            return await _emp.Find(_ => true).ToListAsync();
        }

        public async Task<employeeModel> Get(string id)
        {
            return await _emp.Find(p => p.IdNo == id).FirstOrDefaultAsync();
        }
        public async Task<bool> create(employeeModel emp)
        {
            await _emp.InsertOneAsync(emp);
            return true;
        }
        public async Task<bool> update(String id, employeeModel emp)
        {
            await _emp.ReplaceOneAsync(p => p.IdNo == id, emp);
            return true;
        }
        public async Task<bool> Delete(string id)
        {
            await _emp.DeleteOneAsync(p => p.IdNo == id);
            return true;
        }
        
    }
}