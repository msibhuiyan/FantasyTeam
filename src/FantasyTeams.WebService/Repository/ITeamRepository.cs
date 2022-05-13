using FantasyTeams.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FantasyTeams.Repository
{
    public interface ITeamRepository
    {
        Task<List<Team>> GetAllAsync();
        Task<Team> GetByIdAsync(string id);
        public Task CreateAsync(Team team);
        Task UpdateAsync(string id, Team team);
        Task DeleteAsync(string id);
        Task<Team> GetByNameAsync(string name);
    }
}
