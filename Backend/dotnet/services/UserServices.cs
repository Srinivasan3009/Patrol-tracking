using MongoDB.Driver;
using dotnet.models;
using dotnet.services;
using Microsoft.AspNetCore.Identity;
namespace dotnet.services
{
    public class UserServices
    {
        private readonly IMongoCollection<employeeModel> _user;
       private readonly IPasswordHasher<UpdateUserModel> _passwordHasher;


        public UserServices(IMongoDatabase database, IPasswordHasher<UpdateUserModel> passwordHasher)
        {
            _user = database.GetCollection<employeeModel>("employee");
            _passwordHasher = passwordHasher;
        }
        public async Task<List<UserModel>> GetAll()
        {
            var projection = Builders<employeeModel>.Projection.Include(e => e.IdNo).Include(e => e.Name).Include(e => e.Password).Include(e=>e.plain);
            return await _user.Find(FilterDefinition<employeeModel>.Empty).Project<UserModel>(projection).ToListAsync();
        }
        public async Task<bool> Update(UpdateUserModel model)
        {
            var filter = Builders<employeeModel>.Filter.Eq(e => e.IdNo, model.IdNo);
            model.Password = _passwordHasher.HashPassword(model, model.Password);
            var update = Builders<employeeModel>.Update.Set(e => e.Name, model.Name).Set(e => e.Password, model.Password).Set(e=>e.plain,model.plain);
            await _user.UpdateOneAsync(filter, update);
            return true;
        }
        public async Task<bool> delete(string idNo)
        {
            var user = Builders<employeeModel>.Filter.Eq(e => e.IdNo, idNo);
            var result=await _user.DeleteOneAsync(user);
            return true;
        }

    }
}