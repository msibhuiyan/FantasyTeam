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
        Task<List<T>> GetAllByFilterAsync(FilterDefinition<T> dataFilters);
        Task<List<T>> GetAllByFilterAsync(Expression<Func<T, bool>> dataFilters);
        Task<T> GetAsync(FilterDefinition<T> dataFilters);
        Task<T> GetAsync(Expression<Func<T, bool>> dataFilters);
        Task CreateAsync(T item);
        Task CreateManyAsync(List<T> entities);
        Task UpdateAsync(Expression<Func<T, bool>> dataFilters, T updatedData);
        Task DeleteAsync(Expression<Func<T, bool>> dataFilters);
        Task DeleteManyAsync(Expression<Func<T, bool>> dataFilters);
    }
}
