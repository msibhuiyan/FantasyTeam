using FantasyTeams.Entities;
using FantasyTeams.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FantasyTeams.Repository
{
    public class MarketPlaceRepository : IMarketPlaceRepository
    {
        private readonly IMongoCollection<Player> _collection;
        private readonly DbConfiguration _settings;
        public MarketPlaceRepository(IOptions<DbConfiguration> settings)
        {
            _settings = settings.Value;
            var client = new MongoClient(_settings.ConnectionString);
            var database = client.GetDatabase(_settings.DatabaseName);
            _collection = database.GetCollection<Player>("Player");
        }

        public Task<List<Player>> GetAllAsync()
        {
            return _collection.Find(c => c.ForSale == true).ToListAsync();
        }
        public async Task CreateAsync(Player player)
        {
            await _collection.InsertOneAsync(player).ConfigureAwait(false);
        }
        public Task UpdateAsync(string id, Player team)
        {
            return _collection.ReplaceOneAsync(c => c.Id == id, team);
        }
        public Task<Player> GetByIdAsync(string id)
        {
            return _collection.Find(c => c.Id == id && c.ForSale == true).FirstOrDefaultAsync();
        }

        public Task<List<Player>> GetPlayer(string playerName, string teamName, string country, double value)
        {
            return _collection.Find(c => 
            c.FullName.Contains(playerName) 
            || c.Country == country 
            || c.Value == value).ToListAsync();
        }
    }
}
