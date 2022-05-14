using FantasyTeams.Commands.Team;
using FantasyTeams.Entities;
using FantasyTeams.Models;
using FantasyTeams.Queries;
using System.Threading.Tasks;

namespace FantasyTeams.Contracts
{
    public interface ITeamService
    {
        Task<CommandResponse> CreateNewTeam(CreateTeamCommand createNewTeamCommand, string teamId = null);
        Task<Team> GetTeamInfo(string teamId);
        Task<QueryResponse> GetTeamInfo(GetTeamQuery query);

        Task<QueryResponse> GetAllTeams();
        Task<CommandResponse> UpdateTeamInfo(UpdateTeamCommand updateTeamCommand);
        Task<CommandResponse> DeleteTeam(DeleteTeamCommand deleteTeamCommand);
        Task<CommandResponse> DeleteTeam(string teamId);
        Task<CommandResponse> UpdateTeamInfo(string id, Team team);
        Task<CommandResponse> CreateTeamPlayer(CreateTeamPlayerCommand request);
    }
}
