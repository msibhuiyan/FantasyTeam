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
    public class MarketPlaceService : IMarketPlaceService
    {
        private readonly ILogger<MarketPlaceService> _logger;
        private readonly IMarketPlaceRepository _marketPlacecRepository;
        private readonly ITeamRepository _teamRepository;

        public MarketPlaceService(ILogger<MarketPlaceService> logger,
            IMarketPlaceRepository marketPlacecRepository,
            ITeamRepository teamRepository)
        {
            _logger = logger;
            _marketPlacecRepository = marketPlacecRepository;
            _teamRepository = teamRepository;
        }

        public Task<List<Player>> FindMarketPlacePlayer(FindPlayerQuery findPlayerQuery)
        {
            return _marketPlacecRepository.GetPlayer(
                findPlayerQuery.PlayerName, 
                findPlayerQuery.TeamName, 
                findPlayerQuery.Country,
                findPlayerQuery.Value);
        }

        public async Task<List<Player>> GetAllMarketPlacePlayer()
        {
            return await _marketPlacecRepository.GetAllAsync();
        }

        public async Task<Player> GetMarketPlacePlayer(string PlayerId)
        {
            return await _marketPlacecRepository.GetByIdAsync(PlayerId);
        }

        public async Task PurchasePlayer(PurchasePlayerCommand purchasePlayerCommand)
        {
            Random rnd = new Random();
            var buyerTeamInfo = await _teamRepository.GetByIdAsync(purchasePlayerCommand.TeamId);
            var playerInfo = await _marketPlacecRepository.GetByIdAsync(purchasePlayerCommand.PlayerId);
            if (!string.IsNullOrEmpty(playerInfo.TeamId))
            {
                var sellerTeamInfo = await _teamRepository.GetByIdAsync(playerInfo.TeamId);
                sellerTeamInfo.Budget += playerInfo.AskingPrice;
                sellerTeamInfo.Value -= playerInfo.Value;
                
                if (playerInfo.PlayerType == PlayerType.Defender.ToString())
                {
                    sellerTeamInfo.Defenders = sellerTeamInfo.Defenders.Where(e => e != playerInfo.Id).ToArray();
                }
                else if (playerInfo.PlayerType == PlayerType.Attacker.ToString())
                {
                    sellerTeamInfo.Attackers = sellerTeamInfo.Attackers.Where(e => e != playerInfo.Id).ToArray();
                }
                else if (playerInfo.PlayerType == PlayerType.MidFielder.ToString())
                {
                    sellerTeamInfo.MidFielders = sellerTeamInfo.MidFielders.Where(e => e != playerInfo.Id).ToArray();
                }
                else if (playerInfo.PlayerType == PlayerType.GoalKeeper.ToString())
                {
                    sellerTeamInfo.GoalKeepers = sellerTeamInfo.GoalKeepers.Where(e => e != playerInfo.Id).ToArray();
                }

                await _teamRepository.UpdateAsync(sellerTeamInfo.Id, sellerTeamInfo);
            }

            buyerTeamInfo.Value += playerInfo.Value;
            buyerTeamInfo.Budget -= playerInfo.AskingPrice;
            if (playerInfo.PlayerType == PlayerType.Defender.ToString())
            {
                var playerTypeList = buyerTeamInfo.Defenders.ToList();
                playerTypeList.Add(playerInfo.Id);
                buyerTeamInfo.Defenders = playerTypeList.ToArray();
            }
            else if (playerInfo.PlayerType == PlayerType.Attacker.ToString())
            {
                var playerTypeList = buyerTeamInfo.Attackers.ToList();
                playerTypeList.Add(playerInfo.Id);
                buyerTeamInfo.Attackers = playerTypeList.ToArray();
            }
            else if (playerInfo.PlayerType == PlayerType.MidFielder.ToString())
            {
                var playerTypeList = buyerTeamInfo.MidFielders.ToList();
                playerTypeList.Add(playerInfo.Id);
                buyerTeamInfo.MidFielders = playerTypeList.ToArray();
            }
            else if (playerInfo.PlayerType == PlayerType.GoalKeeper.ToString())
            {
                var playerTypeList = buyerTeamInfo.GoalKeepers.ToList();
                playerTypeList.Add(playerInfo.Id);
                buyerTeamInfo.GoalKeepers = playerTypeList.ToArray();
            }

            await _teamRepository.UpdateAsync(buyerTeamInfo.Id, buyerTeamInfo);


            playerInfo.ForSale = false;
            playerInfo.Value = playerInfo.Value + (playerInfo.Value * rnd.Next(10, 100))/100;
            playerInfo.TeamId = buyerTeamInfo.Id;
            playerInfo.AskingPrice = 0;

            await _marketPlacecRepository.UpdateAsync(playerInfo.Id, playerInfo);

            
        }

        public async Task DeletePlayer(DeletePlayerCommand deletePlayerCommand)
        {
            var player = _marketPlacecRepository.GetByIdAsync(deletePlayerCommand.PlayerId);
            if(player == null)
            {
                // player not found implementation.
            }
            await _marketPlacecRepository.DeleteAsync(deletePlayerCommand.PlayerId);
        }

        public async Task CreateNewMarketPlacePlayer(CreateNewMarketPlacePlayerCommand createNewMarketPlacePlayerCommand)
        {
            Random rnd = new Random();
            var player = new Player();
            player.Id = Guid.NewGuid().ToString();
            player.FirstName = createNewMarketPlacePlayerCommand.FirstName;
            player.LastName = createNewMarketPlacePlayerCommand.LastName;
            player.FullName = player.FirstName + " " + player.LastName;
            player.Country = createNewMarketPlacePlayerCommand.Country;
            player.Value = 1000000;
            player.Age = rnd.Next(18, 40);
            player.ForSale = true;
            player.AskingPrice = createNewMarketPlacePlayerCommand.AskingPrice;
            player.PlayerType = createNewMarketPlacePlayerCommand.PlayerType;
            player.TeamId = null;
            await _marketPlacecRepository.CreateAsync(player);
        }
    }
}
