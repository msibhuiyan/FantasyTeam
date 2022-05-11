using FantasyTeams.Commands;
using FantasyTeams.Contracts;
using FantasyTeams.Entities;
using FantasyTeams.Enums;
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
        public async Task CreateNewPlayer(CreateNewPlayerCommand createNewPlayerCommand)
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
        }

        public async Task<List<Player>> GetAllPlayer()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<List<Player>> CreateNewTeamPlayers(string teamId)
        {
            List<Player> players = new List<Player>();
            players.AddRange(AddGoalKeepers(teamId));
            players.AddRange(AddDefenders(teamId));
            players.AddRange(AddAttackers(teamId));
            players.AddRange(AddMidFielders(teamId));
            
            await _repository.CreateManyAsync(players);
            return players;
        }

        private IEnumerable<Player> AddMidFielders(string teamId)
        {
            Random rnd = new Random();
            List<Player> players = new List<Player>();
            for(int i = 0; i < 6; i++)
            {
                var player = new Player();
                player.Id = Guid.NewGuid().ToString();
                player.FirstName = "Carlos" + rnd.Next(1, 400000000);
                player.LastName = "Linius" + rnd.Next(1, 400000000);
                player.FullName = player.FirstName + " " + player.LastName;
                player.Country = "Barmuda";
                player.Value = 1000000;
                player.Age = rnd.Next(18, 40);
                player.ForSale = false;
                player.AskingPrice = 0;
                player.PlayerType = PlayerType.MidFielder.ToString();
                player.TeamId = teamId;
                players.Add(player);
            }
            return players;
        }

        private IEnumerable<Player> AddAttackers(string teamId)
        {
            Random rnd = new Random();
            List<Player> players = new List<Player>();
            for (int i = 0; i < 5; i++)
            {
                var player = new Player();
                player.Id = Guid.NewGuid().ToString();
                player.FirstName = "Attacker" + rnd.Next(1, 400000000);
                player.LastName = "Linius" + rnd.Next(1, 400000000);
                player.FullName = player.FirstName + " " + player.LastName;
                player.Country = "Barmuda";
                player.Value = 1000000;
                player.Age = rnd.Next(18, 40);
                player.ForSale = false;
                player.AskingPrice = 0;
                player.PlayerType = PlayerType.Attacker.ToString();
                player.TeamId = teamId;
                players.Add(player);
            }
            return players;
        }

        private IEnumerable<Player> AddDefenders(string teamId)
        {
            Random rnd = new Random();
            List<Player> players = new List<Player>();
            for (int i = 0; i < 6; i++)
            {
                var player = new Player();
                player.Id = Guid.NewGuid().ToString();
                player.FirstName = "Defender" + rnd.Next(1, 400000000);
                player.LastName = "Linius" + rnd.Next(1, 400000000);
                player.FullName = player.FirstName + " " + player.LastName;
                player.Country = "Barmuda";
                player.Value = 1000000;
                player.Age = rnd.Next(18, 40);
                player.ForSale = false;
                player.AskingPrice = 0;
                player.PlayerType = PlayerType.Defender.ToString();
                player.TeamId = teamId;
                players.Add(player);
            }
            return players;
        }

        private IEnumerable<Player> AddGoalKeepers(string teamId)
        {
            Random rnd = new Random();
            List<Player> players = new List<Player>();
            for (int i = 0; i < 3; i++)
            {
                var player = new Player();
                player.Id = Guid.NewGuid().ToString();
                player.FirstName = "Keeper" + rnd.Next(1, 400000000);
                player.LastName = "Linius" + rnd.Next(1, 400000000);
                player.FullName = player.FirstName + " " + player.LastName;
                player.Country = "Barmuda";
                player.Value = 1000000;
                player.Age = rnd.Next(18, 40);
                player.ForSale = false;
                player.AskingPrice = 0;
                player.PlayerType = PlayerType.GoalKeeper.ToString();
                player.TeamId = teamId;
                players.Add(player);
            }
            return players;
        }

        public async Task SetPlayerForSale(SetPlayerForSaleCommand setPlayerForSaleCommand)
        {
            var playerInfo  = await _repository.GetByIdAsync(setPlayerForSaleCommand.PlayerId);
            playerInfo.ForSale = true;
            playerInfo.AskingPrice = setPlayerForSaleCommand.AskingPrice;
            await _repository.UpdateAsync(playerInfo.Id, playerInfo);
        }

        public async Task UpdatePlayerInfo(UpdatePlayerCommand updatePlayerCommand)
        {
            var playerInfo = await _repository.GetByIdAsync(updatePlayerCommand.PlayerId);
            if (playerInfo == null)
            {

            }
            playerInfo.FirstName = string.IsNullOrEmpty(updatePlayerCommand.FirstName)?
                playerInfo.FirstName : updatePlayerCommand.FirstName;
            playerInfo.LastName = string.IsNullOrEmpty(updatePlayerCommand.LastName)?
                playerInfo.LastName : updatePlayerCommand.LastName;
            playerInfo.FullName = playerInfo.FirstName + " " + playerInfo.LastName;
            playerInfo.Country = string.IsNullOrEmpty(updatePlayerCommand.Country)?
                playerInfo.Country : updatePlayerCommand.Country;

            await _repository.UpdateAsync(updatePlayerCommand.PlayerId, playerInfo);
        }

        public async Task DeletePlayer(DeletePlayerCommand deletePlayerCommand)
        {
            var player = await _repository.GetByIdAsync(deletePlayerCommand.PlayerId);
            if(player == null)
            {
                return;
            }
            await _repository.DeleteAsync(deletePlayerCommand.PlayerId);
        }

        public async Task UpdatePlayerValue(UpdatePlayerPriceCommand updatePlayerPriceCommand)
        {
            var player = await _repository.GetByIdAsync(updatePlayerPriceCommand.PlayerId);
            if(player == null)
            {
                return;
            }
            player.Value = updatePlayerPriceCommand.PlayerValue;
            await _repository.UpdateAsync(player.Id, player);
        }
        public async Task DeleteTeamPlayers(string teamId)
        {
            await _repository.DeleteManyAsync(teamId);
        }
    }
}
