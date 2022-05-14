using FantasyTeams.Commands;
using FantasyTeams.Contracts;
using FantasyTeams.Entities;
using FantasyTeams.Enums;
using FantasyTeams.Models;
using FantasyTeams.Queries;
using FantasyTeams.Queries.Player;
using FantasyTeams.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyTeams.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly ILogger<PlayerService> _logger;
        private readonly IPlayerRepository _repository;
        public PlayerService(ILogger<PlayerService> logger,
            IPlayerRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }
        public async Task<CommandResponse> CreateNewPlayer(CreateNewPlayerCommand createNewPlayerCommand)
        {
            Random rnd = new Random();

            var player = new Player();
            player.Id = Guid.NewGuid().ToString();
            player.FirstName = createNewPlayerCommand.FirstName;
            player.LastName = createNewPlayerCommand.LastName;
            player.FullName = createNewPlayerCommand.FirstName + " " + createNewPlayerCommand.LastName;
            player.Country = createNewPlayerCommand.Country;
            player.Value = 1000000;
            player.Age = rnd.Next(18, 40);
            player.ForSale = false;
            player.AskingPrice = 0;
            player.PlayerType = "Attacker";
            player.TeamId = null;
            await _repository.CreateAsync(player);
            return CommandResponse.Success();

        }

        public async Task<QueryResponse> GetAllPlayer(GetAllPlayerQuery getAllPlayerQuery)
        {
            var players = new List<Player>();
            if (string.IsNullOrEmpty(getAllPlayerQuery.TeamId))
            {
                players = await _repository.GetAllAsync();
                return QueryResponse.Success(players);
            }
            players =  await _repository.GetAllAsync(getAllPlayerQuery.TeamId);
            return QueryResponse.Success(players);
        }

        public async Task<List<Player>> CreateNewTeamPlayers(Team team)
        {
            List<Player> players = new List<Player>();
            players.AddRange(AddGoalKeepers(team));
            players.AddRange(AddDefenders(team));
            players.AddRange(AddAttackers(team));
            players.AddRange(AddMidFielders(team));

            await _repository.CreateManyAsync(players);
            return players;
        }

        private IEnumerable<Player> AddMidFielders(Team team)
        {
            Random rnd = new Random();
            List<Player> players = new List<Player>();
            for(int i = 0; i < 6; i++)
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

        public async Task<CommandResponse> SetPlayerForSale(SetPlayerForSaleCommand setPlayerForSaleCommand)
        {
            var playerInfo  = await _repository.GetByIdAsync(setPlayerForSaleCommand.PlayerId);
            if(playerInfo.TeamId != setPlayerForSaleCommand.TeamId)
            {
                return CommandResponse.Failure(new string[] { "Can not update other team player price" });
            }
            playerInfo.ForSale = true;
            playerInfo.AskingPrice = setPlayerForSaleCommand.AskingPrice;
            await _repository.UpdateAsync(playerInfo.Id, playerInfo);
            return CommandResponse.Success();
        }

        public async Task<CommandResponse> UpdatePlayerInfo(UpdatePlayerCommand updatePlayerCommand)
        {
            var playerInfo = await _repository.GetByIdAsync(updatePlayerCommand.PlayerId);
            if (playerInfo == null)
            {
                return CommandResponse.Failure(new string[] { "Player not found for update" });
            }
            playerInfo.FirstName = string.IsNullOrEmpty(updatePlayerCommand.FirstName)?
                playerInfo.FirstName : updatePlayerCommand.FirstName;
            playerInfo.LastName = string.IsNullOrEmpty(updatePlayerCommand.LastName)?
                playerInfo.LastName : updatePlayerCommand.LastName;
            playerInfo.FullName = playerInfo.FirstName + " " + playerInfo.LastName;
            playerInfo.Country = string.IsNullOrEmpty(updatePlayerCommand.Country)?
                playerInfo.Country : updatePlayerCommand.Country;

            await _repository.UpdateAsync(updatePlayerCommand.PlayerId, playerInfo);
            return CommandResponse.Success();
        }

        public async Task<CommandResponse> DeletePlayer(DeletePlayerCommand deletePlayerCommand)
        {
            var player = await _repository.GetByIdAsync(deletePlayerCommand.PlayerId);
            if(player == null)
            {
                return CommandResponse.Failure(new string[] { "Player not found to delete" });
            }
            await _repository.DeleteAsync(deletePlayerCommand.PlayerId);
            return CommandResponse.Success();
        }

        public async Task<CommandResponse> UpdatePlayerValue(UpdatePlayerValueCommand updatePlayerPriceCommand)
        {
            var player = await _repository.GetByIdAsync(updatePlayerPriceCommand.PlayerId);
            if(player == null)
            {
                return CommandResponse.Failure(new string[] {"Player not found for update"});
            }
            player.Value = updatePlayerPriceCommand.PlayerValue;
            await _repository.UpdateAsync(player.Id, player);
            return CommandResponse.Success();
        }
        public async Task<CommandResponse> DeleteTeamPlayers(string teamId)
        {
            await _repository.DeleteManyAsync(teamId);
            return CommandResponse.Success();
        }

        public async Task<QueryResponse> GetPlayer(GetPlayerQuery request)
        {
            var player = await _repository.GetByIdAsync(request.PlayerId);
            if( player.TeamId == request.TeamId)
            {
                return QueryResponse.Success(player);
            }
            return QueryResponse.Failure(new string[] {"Can not fetch other team player"});
        }
    }
}
