using FantasyTeams.Commands;
using FantasyTeams.Contracts;
using FantasyTeams.Entities;
using FantasyTeams.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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

        public async Task CreateNewTeamPlayer()
        {
            //Random rnd = new Random();

            //var player = new Player();
            //player.Id = Guid.NewGuid().ToString();
            //player.FirstName = createNewPlayerCommand.FirstName;
            //player.LastName = createNewPlayerCommand.LastName;
            //player.FullName = createNewPlayerCommand.FirstName + " " + createNewPlayerCommand.LastName;
            //player.Country = createNewPlayerCommand.Country;
            //player.Value = 1000000;
            //player.Age = rnd.Next(18, 40);
            //player.ForSale = false;
            //player.AskingPrice = 0;
            //player.PlayerType = "Attacker";
            //player.TeamId = null;
            //await _repository.CreateManyAsync(player);
        }
    }
}
