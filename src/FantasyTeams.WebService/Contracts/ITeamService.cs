using FantasyTeams.Commands;
using FantasyTeams.Entities;
using FantasyTeams.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FantasyTeams.Contracts
{
    public interface ITeamService
    {
        Task<CommandResponse> CreateNewTeam(CreateTeamCommand createNewTeamCommand);
        Task<QueryResponse> GetTeamInfo(string teamId);
        Task<QueryResponse> GetAllTeams();
        Task<CommandResponse> UpdateTeamInfo(UpdateTeamCommand updateTeamCommand);
        Task<CommandResponse> DeleteTeam(DeleteTeamCommand deleteTeamCommand);
        Task<CommandResponse> DeleteTeam(string teamId);
        Task<CommandResponse> UpdateTeamInfo(string id, Team team);
        Task<CommandResponse> CreateTeamPlayer(CreateTeamPlayerCommand request);
    }
}
