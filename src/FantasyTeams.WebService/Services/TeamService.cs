using FantasyTeams.Commands.Team;
using FantasyTeams.Contracts;
using FantasyTeams.Entities;
using FantasyTeams.Enums;
using FantasyTeams.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Linq;
using FantasyTeams.Models;
using FantasyTeams.Queries;

namespace FantasyTeams.Services
{
    public class TeamService : ITeamService
    {
        private readonly ILogger<TeamService> _logger;
        private readonly ITeamRepository _repository;
        private readonly IPlayerService _playerService;
        public TeamService(ILogger<TeamService> logger,
            ITeamRepository repository,
            IPlayerService playerService)
        {
            _logger = logger;
            _repository = repository;
            _playerService = playerService;
        }
        public async Task<CommandResponse> CreateNewTeam(CreateTeamCommand createNewTeamCommand, string teamId = null)
        {
            var team = await _repository.GetByNameAsync(createNewTeamCommand.Name);
            if(team != null)
            {
                return CommandResponse.Failure(new string[] { "Team already exists." });
            }
            team = new Team();
            team.Id = string.IsNullOrEmpty(teamId) ? Guid.NewGuid().ToString() : teamId;
            team.Name = createNewTeamCommand.Name;
            team.Country = createNewTeamCommand.Country;

            var getTeamMembers = await _playerService.CreateNewTeamPlayers(team);

            team.Attackers = getTeamMembers.Where(x=> x.PlayerType == PlayerType.Attacker.ToString()).Select(x=> x.Id).ToArray() ;
            team.Defenders = getTeamMembers.Where(x => x.PlayerType == PlayerType.Defender.ToString()).Select(x => x.Id).ToArray();
            team.MidFielders = getTeamMembers.Where(x => x.PlayerType == PlayerType.MidFielder.ToString()).Select(x => x.Id).ToArray();
            team.GoalKeepers = getTeamMembers.Where(x => x.PlayerType == PlayerType.GoalKeeper.ToString()).Select(x => x.Id).ToArray();
            team.Budget = 5000000;
            team.Value = 20000000;
            await _repository.CreateAsync(team);
            return CommandResponse.Success();
        }

        public async Task<QueryResponse> GetTeamInfo(GetTeamQuery query)
        {
            if (!string.IsNullOrEmpty(query.TeamName))
            {
                var teamByname = await _repository.GetByNameAsync(query.TeamName);
                return QueryResponse.Success(teamByname);
            }
            var teamById = await _repository.GetByIdAsync(query.TeamId);
            return QueryResponse.Success(teamById);
        }

        public async Task<Team> GetTeamInfo(string TeamId)
        {
            return await _repository.GetByIdAsync(TeamId);
        }

        public async Task<QueryResponse> GetAllTeams()
        {
            var teams = await _repository.GetAllAsync();
            return QueryResponse.Success(teams);
        }

        public async Task<CommandResponse> UpdateTeamInfo(UpdateTeamCommand updateTeamCommand)
        {
            var teamInfo = await _repository.GetByIdAsync(updateTeamCommand.TeamId);
            if (teamInfo == null)
            {
                return CommandResponse.Failure(new string[] { "No team found for update" });
            }
            if(string.IsNullOrEmpty(updateTeamCommand.Country) &&
                string.IsNullOrEmpty(updateTeamCommand.Name))
            {
                return CommandResponse.Success();
            }
            if (!string.IsNullOrEmpty(updateTeamCommand.Name))
            {
                var team = await _repository.GetByNameAsync(updateTeamCommand.Name);
                if(team != null)
                {
                    return CommandResponse.Failure(new string[] { "Already a team exists on this name" });
                }
            }
            teamInfo.Country = string.IsNullOrEmpty(updateTeamCommand.Country)? 
                teamInfo.Country : updateTeamCommand.Country;
            teamInfo.Name = string.IsNullOrEmpty(updateTeamCommand.Name)?
                teamInfo.Name : updateTeamCommand.Name;

            await _repository.UpdateAsync(updateTeamCommand.TeamId, teamInfo);
            return CommandResponse.Success();
        }

        public async Task<CommandResponse> DeleteTeam(DeleteTeamCommand deleteTeamCommand)
        {
            var team = await _repository.GetByIdAsync(deleteTeamCommand.TeamId);
            if(team == null)
            {
                return CommandResponse.Failure(new string[] { "No team found for delete" });
            }
            await _playerService.DeleteTeamPlayers(team.Id);
            await _repository.DeleteAsync(deleteTeamCommand.TeamId);
            return CommandResponse.Success();
        }

        public async Task<CommandResponse> DeleteTeam(string teamId)
        {
            var team = await _repository.GetByIdAsync(teamId);
            if (team == null)
            {
                return CommandResponse.Failure(new string[] { "No team found for delete" });
            }
            await _playerService.DeleteTeamPlayers(teamId);
            await _repository.DeleteAsync(teamId);
            return CommandResponse.Success();
        }

        public async Task<CommandResponse> UpdateTeamInfo(string id, Team team)
        {
            await _repository.UpdateAsync(id, team);
            return CommandResponse.Success();
        }

        public async Task<CommandResponse> CreateTeamPlayer(CreateTeamPlayerCommand request)
        {
            var team = await _repository.GetByIdAsync(request.TeamId);
            if(team == null)
            {
                return CommandResponse.Failure(new string[] {"No team found to add this player"});
            }
            return CommandResponse.Failure(new string[] { "Not implemented" });
        }
    }
}
