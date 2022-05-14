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
using System.Collections.Generic;

namespace FantasyTeams.Services
{
    public class TeamService : ITeamService
    {
        private readonly ILogger<TeamService> _logger;
        private readonly ITeamRepository _teamRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IUserRepository _userRepository;
        public TeamService(ILogger<TeamService> logger,
            ITeamRepository teamRepository,
            IPlayerRepository playerRepository,
            IUserRepository userRepository)
        {
            _logger = logger;
            _teamRepository = teamRepository;
            _playerRepository = playerRepository;
            _userRepository = userRepository;
        }
        public async Task<CommandResponse> CreateNewTeam(CreateTeamCommand createNewTeamCommand, string teamId = null)
        {
            var existingTeam = await _teamRepository.GetByNameAsync(createNewTeamCommand.Name);
            if(existingTeam != null)
            {
                return CommandResponse.Failure(new string[] { "Team already exists." });
            }
            var team = new Team();
            team.Id = string.IsNullOrEmpty(teamId) ? Guid.NewGuid().ToString() : teamId;
            team.Name = createNewTeamCommand.Name;
            team.Country = createNewTeamCommand.Country;

            var getTeamMembers = await CreateNewTeamPlayers(team);

            team.Attackers = getTeamMembers.Where(x=> x.PlayerType == PlayerType.Attacker.ToString()).Select(x=> x.Id).ToArray() ;
            team.Defenders = getTeamMembers.Where(x => x.PlayerType == PlayerType.Defender.ToString()).Select(x => x.Id).ToArray();
            team.MidFielders = getTeamMembers.Where(x => x.PlayerType == PlayerType.MidFielder.ToString()).Select(x => x.Id).ToArray();
            team.GoalKeepers = getTeamMembers.Where(x => x.PlayerType == PlayerType.GoalKeeper.ToString()).Select(x => x.Id).ToArray();
            team.Budget = 5000000;
            team.Value = 20000000;
            await _teamRepository.CreateAsync(team);
            return CommandResponse.Success();
        }

        public async Task<List<Player>> CreateNewTeamPlayers(Team team)
        {
            List<Player> players = new List<Player>();
            players.AddRange(AddGoalKeepers(team));
            players.AddRange(AddDefenders(team));
            players.AddRange(AddAttackers(team));
            players.AddRange(AddMidFielders(team));

            await _playerRepository.CreateManyAsync(players);
            return players;
        }
        private IEnumerable<Player> AddMidFielders(Team team)
        {
            Random rnd = new Random();
            List<Player> players = new List<Player>();
            for (int i = 0; i < 6; i++)
            {
                var player = new Player();
                player.Id = Guid.NewGuid().ToString();
                player.FirstName = "MidFielder";
                player.LastName = Guid.NewGuid().ToString();
                player.FullName = player.FirstName + " " + player.LastName;
                player.Country = team.Country;
                player.Value = 1000000;
                player.Age = rnd.Next(18, 40);
                player.ForSale = false;
                player.AskingPrice = 0;
                player.PlayerType = PlayerType.MidFielder.ToString();
                player.TeamId = team.Id;
                player.TeamName = team.Name;
                players.Add(player);
            }
            return players;
        }

        private IEnumerable<Player> AddAttackers(Team team)
        {
            Random rnd = new Random();
            List<Player> players = new List<Player>();
            for (int i = 0; i < 5; i++)
            {
                var player = new Player();
                player.Id = Guid.NewGuid().ToString();
                player.FirstName = "Attacker";
                player.LastName = Guid.NewGuid().ToString();
                player.FullName = player.FirstName + " " + player.LastName;
                player.Country = team.Country;
                player.Value = 1000000;
                player.Age = rnd.Next(18, 40);
                player.ForSale = false;
                player.AskingPrice = 0;
                player.PlayerType = PlayerType.Attacker.ToString();
                player.TeamId = team.Id;
                player.TeamName = team.Name;
                players.Add(player);
            }
            return players;
        }

        private IEnumerable<Player> AddDefenders(Team team)
        {
            Random rnd = new Random();
            List<Player> players = new List<Player>();
            for (int i = 0; i < 6; i++)
            {
                var player = new Player();
                player.Id = Guid.NewGuid().ToString();
                player.FirstName = "Defender";
                player.LastName = Guid.NewGuid().ToString();
                player.FullName = player.FirstName + " " + player.LastName;
                player.Country = team.Country;
                player.Value = 1000000;
                player.Age = rnd.Next(18, 40);
                player.ForSale = false;
                player.AskingPrice = 0;
                player.PlayerType = PlayerType.Defender.ToString();
                player.TeamId = team.Id;
                player.TeamName = team.Name;
                players.Add(player);
            }
            return players;
        }

        private IEnumerable<Player> AddGoalKeepers(Team team)
        {
            Random rnd = new Random();
            List<Player> players = new List<Player>();
            for (int i = 0; i < 3; i++)
            {
                var player = new Player();
                player.Id = Guid.NewGuid().ToString();
                player.FirstName = "Keeper";
                player.LastName = Guid.NewGuid().ToString();
                player.FullName = player.FirstName + " " + player.LastName;
                player.Country = team.Country;
                player.Value = 1000000;
                player.Age = rnd.Next(18, 40);
                player.ForSale = false;
                player.AskingPrice = 0;
                player.PlayerType = PlayerType.GoalKeeper.ToString();
                player.TeamId = team.Id;
                player.TeamName = team.Name;
                players.Add(player);
            }
            return players;
        }

        public async Task<QueryResponse> GetTeamInfo(GetTeamQuery query)
        {
            if (!string.IsNullOrEmpty(query.TeamName))
            {
                var teamByname = await _teamRepository.GetByNameAsync(query.TeamName);
                if(teamByname == null)
                {
                    return QueryResponse.Success(new string[] {"No team Found"});
                }
                return QueryResponse.Success(teamByname);
            }
            var teamById = await _teamRepository.GetByIdAsync(query.TeamId);
            if (teamById == null)
            {
                return QueryResponse.Success(new string[] { "No team Found" });
            }
            return QueryResponse.Success(teamById);
        }

        public async Task<Team> GetTeamInfo(string TeamId)
        {
            return await _teamRepository.GetByIdAsync(TeamId);
        }

        public async Task<QueryResponse> GetAllTeams()
        {
            var teams = await _teamRepository.GetAllAsync();
            return QueryResponse.Success(teams);
        }

        public async Task<CommandResponse> UpdateTeamInfo(UpdateTeamCommand updateTeamCommand)
        {
            var teamInfo = await _teamRepository.GetByIdAsync(updateTeamCommand.TeamId);
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
                var team = await _teamRepository.GetByNameAsync(updateTeamCommand.Name);
                if(team != null)
                {
                    return CommandResponse.Failure(new string[] { "Already a team exists on this name" });
                }
            }
            teamInfo.Country = string.IsNullOrEmpty(updateTeamCommand.Country)? 
                teamInfo.Country : updateTeamCommand.Country;
            teamInfo.Name = string.IsNullOrEmpty(updateTeamCommand.Name)?
                teamInfo.Name : updateTeamCommand.Name;

            await _teamRepository.UpdateAsync(updateTeamCommand.TeamId, teamInfo);
            return CommandResponse.Success();
        }

        public async Task<CommandResponse> DeleteTeam(DeleteTeamCommand deleteTeamCommand)
        {
            var team = await _teamRepository.GetByIdAsync(deleteTeamCommand.TeamId);
            if(team == null)
            {
                return CommandResponse.Failure(new string[] { "No team found for delete" });
            }
            await _playerRepository.DeleteManyAsync(team.Id);
            await _teamRepository.DeleteAsync(deleteTeamCommand.TeamId);
            await _userRepository.DeleteByTeamIdAsync(deleteTeamCommand.TeamId);
            return CommandResponse.Success();
        }

        public async Task<CommandResponse> UpdateTeamInfo(string id, Team team)
        {
            await _teamRepository.UpdateAsync(id, team);
            return CommandResponse.Success();
        }

        public async Task<CommandResponse> CreateTeamPlayer(CreateTeamPlayerCommand request)
        {
            var team = await _teamRepository.GetByIdAsync(request.TeamId);
            if(team == null)
            {
                return CommandResponse.Failure(new string[] {"No team found to add this player"});
            }
            return CommandResponse.Failure(new string[] { "Not implemented" });
        }
    }
}
