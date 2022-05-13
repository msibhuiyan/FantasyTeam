using FantasyTeams.Commands;
using FantasyTeams.Contracts;
using FantasyTeams.Entities;
using FantasyTeams.Enums;
using FantasyTeams.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using FantasyTeams.Models;
using Newtonsoft.Json;

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
        public async Task<CommandResponse> CreateNewTeam(CreateNewTeamCommand createNewTeamCommand)
        {
            var team = new Team();
            team.Id = Guid.NewGuid().ToString();

            var getTeamMembers = await _playerService.CreateNewTeamPlayers(team.Id);

            team.Name = createNewTeamCommand.Name;
            team.Country = createNewTeamCommand.Country;
            team.Attackers = getTeamMembers.Where(x=> x.PlayerType == PlayerType.Attacker.ToString()).Select(x=> x.Id).ToArray() ;
            team.Defenders = getTeamMembers.Where(x => x.PlayerType == PlayerType.Defender.ToString()).Select(x => x.Id).ToArray();
            team.MidFielders = getTeamMembers.Where(x => x.PlayerType == PlayerType.MidFielder.ToString()).Select(x => x.Id).ToArray();
            team.GoalKeepers = getTeamMembers.Where(x => x.PlayerType == PlayerType.GoalKeeper.ToString()).Select(x => x.Id).ToArray();
            team.Budget = 5000000;
            team.Value = 20000000;
            await _repository.CreateAsync(team);
            //var json = JsonConvert.SerializeObject(team);
            return new CommandResponse
            {
                Message = "Team " + team.Name + " created.",
                IsSuccess = true
            };
        }

        public async Task<Team> GetTeamInfo(string TeamId)
        {
            return await _repository.GetByIdAsync(TeamId);
        }

        public async Task<List<Team>> GetAllTeams()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<CommandResponse> UpdateTeamInfo(UpdateTeamCommand updateTeamCommand)
        {
            var teamInfo = await _repository.GetByIdAsync(updateTeamCommand.TeamId);
            if(teamInfo == null)
            {
                return new CommandResponse
                {
                    Message = "No team found for update",
                    IsSuccess = false
                };
            }
            teamInfo.Country = string.IsNullOrEmpty(updateTeamCommand.Country)? 
                teamInfo.Country : updateTeamCommand.Country;
            teamInfo.Name = string.IsNullOrEmpty(updateTeamCommand.Name)?
                teamInfo.Name : updateTeamCommand.Name;

            await _repository.UpdateAsync(updateTeamCommand.TeamId, teamInfo);
            return new CommandResponse
            {
                Message = "Team "+ teamInfo.Name + " updated",
                IsSuccess = true
            };
        }

        public async Task<CommandResponse> DeleteTeam(DeleteTeamCommand deleteTeamCommand)
        {
            var team = await _repository.GetByIdAsync(deleteTeamCommand.TeamId);
            if(team == null)
            {
                return new CommandResponse
                {
                    Message = "No team found for delete",
                    IsSuccess = false
                };
            }
            await _playerService.DeleteTeamPlayers(team.Id);
            await _repository.DeleteAsync(deleteTeamCommand.TeamId);
            return new CommandResponse
            {
                Message = "Team " + team.Name + " deleted",
                IsSuccess = true
            };
        }

        public async Task<CommandResponse> DeleteTeam(string teamId)
        {
            var team = await _repository.GetByIdAsync(teamId);
            if (team == null)
            {
                return new CommandResponse
                {
                    Message = "No team found for delete",
                    IsSuccess = false
                };
            }
            await _playerService.DeleteTeamPlayers(teamId);
            await _repository.DeleteAsync(teamId);
            return new CommandResponse
            {
                Message = "Team " + team.Name + " deleted",
                IsSuccess = true
            };
        }

        public async Task<CommandResponse> UpdateTeamInfo(string id, Team team)
        {
            await _repository.UpdateAsync(id, team);
            return new CommandResponse
            {
                Message = "Team " + team.Name + " updated",
                IsSuccess = true
            };
        }
    }
}
