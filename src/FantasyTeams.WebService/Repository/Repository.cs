using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FantasyTeams.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace FantasyTeams.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IMongoCollection<T> _collection;
        private readonly DbConfiguration _settings;
        public Repository(IOptions<DbConfiguration> settings)
        {
            _settings = settings.Value;
            var client = new MongoClient(_settings.ConnectionString);
            var database = client.GetDatabase(_settings.DatabaseName);
            _collection = database.GetCollection<T>(typeof(T).Name);
        }
        public Task<List<T>> GetAllAsync()
        {
            return _collection.Find(x => true).ToListAsync();
        }
        public Task<T> GetAsync(FilterDefinition<T> dataFilters)
        {
            return _collection.Find<T>(dataFilters).FirstOrDefaultAsync();
        }

        public Task<T> GetAsync(Expression<Func<T, bool>> dataFilters)
        {
            return _collection.Find<T>(dataFilters).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(T player)
        {
            await _collection.InsertOneAsync(player).ConfigureAwait(false);
        }
        public Task UpdateAsync(Expression<Func<T, bool>> dataFilters, T updatedData)
        {
            var filter = Builders<T>.Filter.Where(dataFilters);
            return _collection.ReplaceOneAsync(filter, updatedData);
            
        }
        public Task DeleteAsync(Expression<Func<T, bool>> dataFilters)
        {
            return _collection.DeleteOneAsync(dataFilters);
        }

        public async Task CreateManyAsync(List<T> players)
        {
            await _collection.InsertManyAsync(players);
        }

        public async Task DeleteManyAsync(Expression<Func<T, bool>> dataFilters)
        {
            await _collection.DeleteManyAsync(dataFilters);
        }

        public async Task<List<T>> GetAllByFilterAsync(FilterDefinition<T> dataFilters)
        {
            return await _collection.Find(dataFilters).ToListAsync();
        }

    }
}
