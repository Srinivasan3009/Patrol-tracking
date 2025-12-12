using MongoDB.Driver;
using dotnet.models;

namespace dotnet.services
{
    public class LocationService
    {
        private readonly IMongoCollection<LocationModel> _locations;

        public LocationService(IMongoDatabase database)
        {
            _locations = database.GetCollection<LocationModel>("locations");
        }

        public async Task<List<LocationModel>> GetAllAsync() =>
            await _locations.Find(_ => true).ToListAsync();

        public async Task<LocationModel?> GetByCodeAsync(string code) =>
            await _locations.Find(loc => loc.LocationCode == code).FirstOrDefaultAsync();

        public async Task AddAsync(LocationModel location)  {
        var count = await _locations.CountDocumentsAsync(FilterDefinition<LocationModel>.Empty);
        var newCode = $"LOC{(count + 1).ToString("D3")}";

        location.LocationCode = newCode;

        await _locations.InsertOneAsync(location);
    }

        public async Task<bool> UpdateAsync(string code, LocationModel updated)
         {
        var update = Builders<LocationModel>.Update
            .Set("locationName", updated.LocationName)
            .Set("description", updated.Description)
            .Set("coordinates.latitude", updated.Coordinates.Latitude)
            .Set("coordinates.longitude", updated.Coordinates.Longitude);

        var result = await _locations.UpdateOneAsync(
            Builders<LocationModel>.Filter.Eq("locationCode", code),
            update
        );

        return result.ModifiedCount > 0;
    }


        public async Task<bool> DeleteAsync(string code)
        {
            var result = await _locations.DeleteOneAsync(loc => loc.LocationCode == code);
            return result.DeletedCount > 0;
        }
    }
}
