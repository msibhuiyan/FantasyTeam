using FantasyTeams.Commands;
using FantasyTeams.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FantasyTeams.Contracts
{
    public interface ITeamService
    {
        Task CreateNewTeam(CreateNewTeamCommand createNewTeamCommand);
        Task<Team> GetTeamInfo(string teamId);
        Task<List<Team>> GetAllTeams();
        Task UpdateTeamInfo(UpdateTeamCommand updateTeamCommand);
        Task DeleteTeam(DeleteTeamCommand deleteTeamCommand);
        Task DeleteTeam(string teamId);
        Task UpdateTeamInfo(string id, Team team);
    }
}
