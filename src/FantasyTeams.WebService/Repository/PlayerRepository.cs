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
        public async Task<Player> CreateAsync(CreateNewPlayerCommand createNewPlayerCommand)
        {
            Random rnd = new Random();

            var player = new Player();
            player.Id = Guid.NewGuid().ToString();
            player.FirstName = createNewPlayerCommand.FirstName;
            player.LastName = createNewPlayerCommand.LastName;
            player.FullName = createNewPlayerCommand.FirstName + " " + createNewPlayerCommand.LastName;
            player.Country = createNewPlayerCommand.Country;
            player.Value = 1000000;
            player.Age = rnd.Next(18, 40);
            player.ForSale = false;
            player.AskingPrice = 0;
            player.PlayerType = "Attacker";
            player.TeamId = null;

            await _collection.InsertOneAsync(player).ConfigureAwait(false);
            return player;
        }
        public Task UpdateAsync(string id, Player team)
        {
            return _collection.ReplaceOneAsync(c => c.Id == id, team);
        }
        public Task DeleteAsync(string id)
        {
            return _collection.DeleteOneAsync(c => c.Id == id);
        }

    }
}
