using FantasyTeams.Commands;
using FantasyTeams.Contracts;
using FantasyTeams.Entities;
using FantasyTeams.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FantasyTeams.Services
{
    public class TeamService : ITeamService
    {
        private readonly ILogger<TeamService> _logger;
        private readonly ITeamRepository _repository;
        public TeamService(ILogger<TeamService> logger,
            ITeamRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }
        public async Task CreateNewTeam(CreateNewTeamCommand createNewTeamCommand)
        {
            var team = new Team();
            team.Id = Guid.NewGuid().ToString();
            team.Name = createNewTeamCommand.Name;
            team.Country = createNewTeamCommand.Country;
            team.Attackers = new string[] { };
            team.Defenders = new string[] { };
            team.MidFielders = new string[] { };
            team.GoalKeepers = new string[] { };
            team.Budget = 5000000;
            team.Value = 20000000;
            await _repository.CreateAsync(team);
        }
    }
}
