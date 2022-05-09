using FantasyTeams.Commands;
using FantasyTeams.Entities;
using FantasyTeams.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FantasyTeams.Repository
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly IMongoCollection<Player> _collection;
        private readonly DbConfiguration _settings;
        public PlayerRepository(IOptions<DbConfiguration> settings)
        {
            _settings = settings.Value;
            var client = new MongoClient(_settings.ConnectionString);
            var database = client.GetDatabase(_settings.DatabaseName);
            _collection = database.GetCollection<Player>("Player");
        }

        public Task<List<Player>> GetAllAsync()
        {
            return _collection.Find(c => true).ToListAsync();
        }
        public Task<Player> GetByIdAsync(string id)
        {
            return _collection.Find(c => c.Id == id).FirstOrDefaultAsync();
        }
        public async Task CreateAsync(Player player)
        {
            

            await _collection.InsertOneAsync(player).ConfigureAwait(false);
        }
        public Task UpdateAsync(string id, Player team)
        {
            return _collection.ReplaceOneAsync(c => c.Id == id, team);
        }
        public Task DeleteAsync(string id)
        {
            return _collection.DeleteOneAsync(c => c.Id == id);
        }

        public void CreateMany(List<Player> players)
        {
             _collection.InsertMany(players);
        }
    }
}
