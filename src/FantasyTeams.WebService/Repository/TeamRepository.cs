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
    public class TeamRepository : ITeamRepository
    {
        private readonly IMongoCollection<Team> _collection;
        private readonly DbConfiguration _settings;
        public TeamRepository(IOptions<DbConfiguration> settings)
        {
            _settings = settings.Value;
            var client = new MongoClient(_settings.ConnectionString);
            var database = client.GetDatabase(_settings.DatabaseName);
            _collection = database.GetCollection<Team>("Team");
        }

        public Task<List<Team>> GetAllAsync()
        {
            return _collection.Find(c => true).ToListAsync();
        }
        public Task<Team> GetByIdAsync(string id)
        {
            return _collection.Find(c => c.Id == id).FirstOrDefaultAsync();
        }
        public async Task<Team> CreateAsync(CreateNewTeamCommand createNewTeamCommand)
        {
            var team = new Team();
            team.Id = Guid.NewGuid().ToString();
            team.Name = createNewTeamCommand.Name;
            team.Country = createNewTeamCommand.Country;
            team.Attackers = new string[] { };
            team.Defenders = new string[] { };
            team.MidFielders = new string[] { };
            team.GoalKeepers = new string[] { };
            team.Budget = 5000000;
            team.Value = 20000000;

            await _collection.InsertOneAsync(team).ConfigureAwait(false);
            return team;
        }
        public Task UpdateAsync(string id, Team team)
        {
            return _collection.ReplaceOneAsync(c => c.Id == id, team);
        }
        public Task DeleteAsync(string id)
        {
            return _collection.DeleteOneAsync(c => c.Id == id);
        }

    }
}
