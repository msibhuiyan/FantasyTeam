using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FantasyTeams.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> dataFilters);
        Task<T> GetByIdAsync(FilterDefinitionBuilder<Func<T>> dataFilters);
        Task CreateAsync(T item);
        Task CreateManyAsync(List<T> entities);
        Task UpdateAsync(Expression<Func<T, bool>> dataFilters, UpdateDefinition<T> update);
        Task DeleteAsync(Expression<Func<T, bool>> dataFilters);
        Task DeleteManyAsync(Expression<Func<T, bool>> dataFilters);
        Task<T> GetByNameAsync(Expression<Func<T, bool>> dataFilters);
    }
}
