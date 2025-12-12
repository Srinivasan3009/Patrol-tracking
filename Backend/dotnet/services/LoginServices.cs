// Refactored LoginServices using only employeeModel

using MongoDB.Driver;
using dotnet.models;
using dotnet.services;
using Microsoft.AspNetCore.Identity;

namespace dotnet.services
{
    public class LoginServices
    {
        private readonly IMongoCollection<employeeModel> _employeeCollection;
        private readonly EmailHelper _emailHelper;
        private readonly IPasswordHasher<employeeModel> _passwordHasher;

        public LoginServices(IMongoDatabase database, EmailHelper emailHelper, IPasswordHasher<employeeModel> passwordHasher)
        {
            _employeeCollection = database.GetCollection<employeeModel>("employee");
            _emailHelper = emailHelper;
            _passwordHasher = passwordHasher;
        }

        public async Task<string> Signup(employeeModel emp)
        {
            var existing = await _employeeCollection.Find(e => e.Name == emp.Name || e.email == emp.email).FirstOrDefaultAsync();
            if (existing != null)
            {
                return "User already exists!";
            }

            var count = await _employeeCollection.CountDocumentsAsync(FilterDefinition<employeeModel>.Empty);
            var newIdNo = $"{(count + 1).ToString("D3")}";
            while (await _employeeCollection.Find(e => e.IdNo == newIdNo).AnyAsync())
            {
                count++;
                newIdNo = $"{(count + 1).ToString("D3")}";
            }

            emp.Id = string.Empty;
            emp.IdNo = newIdNo;
            emp.Password = _passwordHasher.HashPassword(emp, emp.Password);

            await _employeeCollection.InsertOneAsync(emp);
            return "Success";
        }

        public async Task<employeeModel?> LoginAsync(string Username, string password)
        {
            var user = await _employeeCollection.Find(e => e.email == Username).FirstOrDefaultAsync();
            if (user == null) return null;

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
            if (result != PasswordVerificationResult.Success) return null;

            var otp = new Random().Next(1000, 9999).ToString();

            var update = Builders<employeeModel>.Update
                .Set(u => u.Otp, otp)
                .Set(u => u.OtpGeneratedAt, DateTime.UtcNow);

            await _employeeCollection.UpdateOneAsync(e => e.email == Username, update);

            user.Otp = otp;
            user.OtpGeneratedAt = DateTime.UtcNow;
            await _emailHelper.SendOtpEmailAsync(user.email, otp);

            return user;
        }

        public async Task<bool> VerifyOtpAsync(string email, string otp)
        {
            var user = await _employeeCollection.Find(e => e.email == email).FirstOrDefaultAsync();
            if (user == null || user.Otp != otp)
                return false;

            if (user.OtpGeneratedAt.HasValue && user.OtpGeneratedAt.Value.AddMinutes(5) < DateTime.UtcNow)
            {
                return false;
            }

            var update = Builders<employeeModel>.Update
                .Set(u => u.Otp, null)
                .Set(u => u.OtpGeneratedAt, null);

            await _employeeCollection.UpdateOneAsync(e => e.email == email, update);

            return true;
        }
    }
}
