using FantasyTeams.Queries;
using FantasyTeams.Contracts;
using FantasyTeams.Entities;
using FantasyTeams.Enums;
using FantasyTeams.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using FantasyTeams.Models;
using MongoDB.Driver;
using FantasyTeams.Commands.MarketPlace;

namespace FantasyTeams.Services
{
    public class MarketPlaceService : IMarketPlaceService
    {
        private readonly ILogger<MarketPlaceService> _logger;
        private readonly IRepository<Team> _teamRepository;
        private readonly IRepository<Player> _playerRepository;

        public MarketPlaceService(ILogger<MarketPlaceService> logger,
            IRepository<Team> teamRepository,
            IRepository<Player> playerRepository)
        {
            _logger = logger;
            _teamRepository = teamRepository;
            _playerRepository = playerRepository;
        }

        public async Task<QueryResponse> FindMarketPlacePlayer(FindPlayerQuery findPlayerQuery)
        {
            var findPlayerfilter = GetFilteredPlayerAsync(findPlayerQuery);

            var filteredPlayer = await this._playerRepository.GetAsync(findPlayerfilter);
            
            return QueryResponse.Success(filteredPlayer);
        }

        private FilterDefinition<Player> GetFilteredPlayerAsync(FindPlayerQuery findPlayerQuery)
        {
            var combinedFilter = Builders<Player>.Filter.Empty;
            combinedFilter = new FilterDefinitionBuilder<Player>().Eq(x => x.ForSale, true);
            if (!string.IsNullOrEmpty(findPlayerQuery.TeamName))
            {
                combinedFilter &= new FilterDefinitionBuilder<Player>().Eq(x => x.TeamName, findPlayerQuery.TeamName);
            }
            if (!string.IsNullOrEmpty(findPlayerQuery.Country))
            {
                combinedFilter &= new FilterDefinitionBuilder<Player>().Eq(x => x.Country, findPlayerQuery.Country);
            }
            if (!string.IsNullOrEmpty(findPlayerQuery.PlayerName))
            {
                combinedFilter &= new FilterDefinitionBuilder<Player>().Eq(x => x.FullName, findPlayerQuery.PlayerName);
            }
            if (findPlayerQuery.Value != null)
            {
                combinedFilter &= new FilterDefinitionBuilder<Player>().Eq(x => x.AskingPrice, findPlayerQuery.Value);
            }

            return combinedFilter;
        }

        public async Task<QueryResponse> GetAllMarketPlacePlayer()
        {
            var marketPlacePlayers = await _playerRepository.GetAllByFilterAsync(x => x.ForSale == true);
            return QueryResponse.Success(marketPlacePlayers);
        }

        public async Task<QueryResponse> GetMarketPlacePlayer(string PlayerId)
        {
            var marketPlacePlayer = await _playerRepository.GetAsync(x=> x.Id == PlayerId && x.ForSale == true);
            return QueryResponse.Success(marketPlacePlayer);
        }

        public async Task<CommandResponse> PurchasePlayer(PurchasePlayerCommand purchasePlayerCommand)
        {
            Random rnd = new Random();
            var buyerTeamInfo = await _teamRepository.GetAsync(x=> x.Id == purchasePlayerCommand.TeamId);
            if(buyerTeamInfo == null)
            {
                return CommandResponse.Failure(new string[] { "Buyer team doesn't exists" });
            }
            var playerInfo = await _playerRepository.GetAsync(x=> x.Id == purchasePlayerCommand.PlayerId);
            if(playerInfo == null)
            {
                return CommandResponse.Failure(new string[] { "The player is no longer in market place" });
            }
            if(buyerTeamInfo.Id == playerInfo.TeamId)
            {
                return CommandResponse.Failure(new string[] { "Can not purchase your own player" });
            }
            if (buyerTeamInfo.Budget < playerInfo.AskingPrice)
            {
                return CommandResponse.Failure(new string[] { "Dont have enough budget to purchase" });
            }
            if (!string.IsNullOrEmpty(playerInfo.TeamId))
            {
                var sellerTeamInfo = await _teamRepository.GetAsync(x=> x.Id == playerInfo.TeamId);
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

                await _teamRepository.UpdateAsync(x=> x.Id == sellerTeamInfo.Id, sellerTeamInfo);
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
            playerInfo.TeamName = buyerTeamInfo.Name;

            buyerTeamInfo.Value += playerInfo.Value;
            buyerTeamInfo.Budget -= playerInfo.AskingPrice;

            playerInfo.AskingPrice = 0;

            await _playerRepository.UpdateAsync(x => x.Id == playerInfo.Id, playerInfo);
            await _teamRepository.UpdateAsync(x=> x.Id == buyerTeamInfo.Id, buyerTeamInfo);
            return CommandResponse.Success();
        }

        public async Task<CommandResponse> DeletePlayer(DeleteMarketPlacePlayerCommand deletePlayerCommand)
        {
            var player = await _playerRepository.GetAsync(x=> x.Id == deletePlayerCommand.PlayerId && x.ForSale == true);
            if(player == null)
            {
                return CommandResponse.Failure(new string[] {"Player not found in marketplace for deletion"});
            }
            if (!string.IsNullOrEmpty(player.TeamId))
            {
                return CommandResponse.Failure(new string[] { "Can not delete market place player associated with a team" });
            }
            await _playerRepository.DeleteAsync(x=>x.Id == deletePlayerCommand.PlayerId);
            return CommandResponse.Success();
        }

        public async Task<CommandResponse> CreateNewMarketPlacePlayer(CreateMarketPlacePlayerCommand createNewMarketPlacePlayerCommand)
        {
            Random rnd = new Random();
            var fullName = createNewMarketPlacePlayerCommand.FirstName + " " + createNewMarketPlacePlayerCommand.LastName;
            var player = await _playerRepository.GetAsync(x=> x.FullName == fullName);
            if(player != null)
            {
                return CommandResponse.Failure(new string[] { "This player already exists" });
            }
            player = new Player();
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
            await _playerRepository.CreateAsync(player);
            return CommandResponse.Success();
        }
    }
}
