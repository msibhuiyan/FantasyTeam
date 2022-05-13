using FantasyTeams.Commands;
using FantasyTeams.Queries;
using FantasyTeams.Contracts;
using FantasyTeams.Entities;
using FantasyTeams.Enums;
using FantasyTeams.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyTeams.Models;
using MongoDB.Driver;
using MongoDB.Bson;

namespace FantasyTeams.Services
{
    public class MarketPlaceService : IMarketPlaceService
    {
        private readonly ILogger<MarketPlaceService> _logger;
        private readonly IMarketPlaceRepository _marketPlacecRepository;
        private readonly ITeamService _teamService;

        public MarketPlaceService(ILogger<MarketPlaceService> logger,
            IMarketPlaceRepository marketPlacecRepository,
            ITeamService teamService)
        {
            _logger = logger;
            _marketPlacecRepository = marketPlacecRepository;
            _teamService = teamService;
        }

        public async Task<QueryResponse> FindMarketPlacePlayer(FindPlayerQuery findPlayerQuery)
        {
            var filter1 = Builders<Player>.Filter.Eq("FullName", findPlayerQuery.PlayerName);
            var filter2 = Builders<Player>.Filter.Eq("Country", findPlayerQuery.Country);
            //var filter3 = Builders<BsonDocument>.Filter.Eq("PlayerType", findPlayerQuery.TeamName);
            var filter4 = Builders<Player>.Filter.Eq("AskingPrice", findPlayerQuery.Value);
            var andFilter = filter1 | filter2 | filter4;
            var data = _marketPlacecRepository.GetFilteredPlayerAsync(andFilter);
            //var cursor = ;
            var searchedPlayers = await _marketPlacecRepository.GetPlayer(
                findPlayerQuery.PlayerName, 
                findPlayerQuery.TeamName, 
                findPlayerQuery.Country,
                findPlayerQuery.Value);
            return QueryResponse.Success(data.Result);
        }

        public async Task<QueryResponse> GetAllMarketPlacePlayer()
        {
            var marketPlacePlayers = await _marketPlacecRepository.GetAllAsync();
            return QueryResponse.Success(marketPlacePlayers);
        }

        public async Task<QueryResponse> GetMarketPlacePlayer(string PlayerId)
        {
            var marketPlacePlayer = await _marketPlacecRepository.GetByIdAsync(PlayerId);
            return QueryResponse.Success(marketPlacePlayer);
        }

        public async Task<CommandResponse> PurchasePlayer(PurchasePlayerCommand purchasePlayerCommand)
        {
            Random rnd = new Random();
            var buyerTeamInfo = await _teamService.GetTeamInfo(purchasePlayerCommand.TeamId);
            var playerInfo = await _marketPlacecRepository.GetByIdAsync(purchasePlayerCommand.PlayerId);
            if(buyerTeamInfo.Id == playerInfo.TeamId)
            {
                return CommandResponse.Failure(new string[] { "Can not purchase your own player" });
            }
            if (!string.IsNullOrEmpty(playerInfo.TeamId))
            {
                var sellerTeamInfo = await _teamService.GetTeamInfo(playerInfo.TeamId);
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

                await _teamService.UpdateTeamInfo(sellerTeamInfo.Id, sellerTeamInfo);
            }

            
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

            playerInfo.ForSale = false;
            playerInfo.Value = playerInfo.Value + (playerInfo.Value * rnd.Next(10, 100))/100;
            playerInfo.TeamId = buyerTeamInfo.Id;
            playerInfo.AskingPrice = 0;

            buyerTeamInfo.Value += playerInfo.Value;
            buyerTeamInfo.Budget -= playerInfo.AskingPrice;

            await _marketPlacecRepository.UpdateAsync(playerInfo.Id, playerInfo);
            await _teamService.UpdateTeamInfo(buyerTeamInfo.Id, buyerTeamInfo);
            return CommandResponse.Success();
            
        }

        public async Task<CommandResponse> DeletePlayer(DeletePlayerCommand deletePlayerCommand)
        {
            var player = _marketPlacecRepository.GetByIdAsync(deletePlayerCommand.PlayerId);
            if(player == null)
            {
                return CommandResponse.Failure(new string[] {"Player not found in marketplace for deletion"});
            }
            await _marketPlacecRepository.DeleteAsync(deletePlayerCommand.PlayerId);
            return CommandResponse.Success();
        }

        public async Task<CommandResponse> CreateNewMarketPlacePlayer(CreateMarketPlacePlayerCommand createNewMarketPlacePlayerCommand)
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
            return CommandResponse.Success();
        }
    }
}
