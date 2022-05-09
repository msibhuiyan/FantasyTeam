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

            playerInfo.ForSale = false;
            playerInfo.Value = playerInfo.Value + (playerInfo.Value * rnd.Next(10, 100))/100;
            playerInfo.TeamId = buyerTeamInfo.Id;

            await _marketPlacecRepository.UpdateAsync(playerInfo.Id, playerInfo);

            buyerTeamInfo.Value += playerInfo.Value;
            if(playerInfo.PlayerType == PlayerType.Defender.ToString())
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
        }
    }
}
