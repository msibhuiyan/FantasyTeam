using FantasyTeams.Commands;
using FantasyTeams.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FantasyTeams.Repository
{
    public interface ITeamRepository
    {
        Task<List<Team>> GetAllAsync();
        Task<Team> GetByIdAsync(string id);
        Task<Team> CreateAsync(CreateNewTeamCommand createNewTeamCommand);
        Task UpdateAsync(string id, Team team);
        Task DeleteAsync(string id);
    }
}
