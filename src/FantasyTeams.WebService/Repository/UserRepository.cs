using FantasyTeams.Entities;
using FantasyTeams.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FantasyTeams.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _collection;
        private readonly DbConfiguration _settings;
        public UserRepository(IOptions<DbConfiguration> settings)
        {
            _settings = settings.Value;
            var client = new MongoClient(_settings.ConnectionString);
            var database = client.GetDatabase(_settings.DatabaseName);
            _collection = database.GetCollection<User>("User");
        }
        public async Task CreateAsync(User user)
        {
            await _collection.InsertOneAsync(user);
        }

        public Task UpdateAsync(string id, User user)
        {
            return _collection.ReplaceOneAsync(c => c.Id == id, user);
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _collection.Find(c => true).ToListAsync();
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _collection.Find(c => c.Email == email).FirstOrDefaultAsync();
        }
        public async Task DeleteAsync(string email)
        {
            await _collection.DeleteOneAsync(c => c.Email == email);
        }
        public async Task DeleteByTeamIdAsync(string teamId)
        {
            await _collection.DeleteOneAsync(c => c.TeamId == teamId);
        }
        public async Task<User> GetByTeamIdAsync(string teamId)
        {
            return await _collection.Find(x => x.TeamId == teamId).FirstOrDefaultAsync();
        }
        public async Task<User> GetByIdAsync(string Id)
        {
            return await _collection.Find(x => x.Id == Id).FirstOrDefaultAsync();
        }

        public async Task<List<User>> GetAllUnAssignedTeamAsync()
        {
            return await _collection.Find(x => x.TeamId == "" || x.TeamId == null).ToListAsync();
        }
    }
}
