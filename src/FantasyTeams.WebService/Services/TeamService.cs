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
        public async Task CreateNewTeam(CreateNewTeamCommand createNewTeamCommand)
        {
            var team = new Team();
            team.Id = Guid.NewGuid().ToString();

            var getTeamMembers = _playerService.CreateNewTeamPlayers(team.Id);

            team.Name = createNewTeamCommand.Name;
            team.Country = createNewTeamCommand.Country;
            team.Attackers = getTeamMembers.Where(x=> x.PlayerType == PlayerType.Attacker.ToString()).Select(x=> x.Id).ToArray() ;
            team.Defenders = getTeamMembers.Where(x => x.PlayerType == PlayerType.Defender.ToString()).Select(x => x.Id).ToArray();
            team.MidFielders = getTeamMembers.Where(x => x.PlayerType == PlayerType.MidFielder.ToString()).Select(x => x.Id).ToArray();
            team.GoalKeepers = getTeamMembers.Where(x => x.PlayerType == PlayerType.GoalKeeper.ToString()).Select(x => x.Id).ToArray();
            team.Budget = 5000000;
            team.Value = 20000000;
            await _repository.CreateAsync(team);
        }

        public async Task<Team> GetTeamInfo(string TeamId)
        {
            return await _repository.GetByIdAsync(TeamId);
        }

        public async Task<List<Team>> GetAllTeams()
        {
            return await _repository.GetAllAsync();
        }

        public async Task UpdateTeamInfo(UpdateTeamCommand updateTeamCommand)
        {
            var teamInfo = await _repository.GetByIdAsync(updateTeamCommand.TeamId);
            if(teamInfo == null)
            {

            }
            teamInfo.Country = updateTeamCommand.Country;
            teamInfo.Name = updateTeamCommand.Name;

            await _repository.UpdateAsync(updateTeamCommand.TeamId, teamInfo);
        }
    }
}
