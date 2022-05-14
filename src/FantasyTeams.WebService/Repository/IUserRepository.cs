using FantasyTeams.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FantasyTeams.Repository
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        Task<User> GetByEmailAsync(string email);
        public Task CreateAsync(User user);
        Task UpdateAsync(string id, User user);
        Task DeleteAsync(string email);
        Task DeleteByTeamIdAsync(string teamId);
    }
}
