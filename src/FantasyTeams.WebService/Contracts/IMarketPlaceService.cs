using FantasyTeams.Commands;
using FantasyTeams.Queries;
using FantasyTeams.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using FantasyTeams.Models;

namespace FantasyTeams.Contracts
{
    public interface IMarketPlaceService
    {
        Task<QueryResponse> GetAllMarketPlacePlayer();
        Task<QueryResponse> GetMarketPlacePlayer(string PlayerId);
        Task<CommandResponse> PurchasePlayer(PurchasePlayerCommand purchasePlayerCommand);
        Task<QueryResponse> FindMarketPlacePlayer(FindPlayerQuery findPlayerQuery);
        Task<CommandResponse> DeletePlayer(DeletePlayerCommand deletePlayerCommand);
        Task<CommandResponse> CreateNewMarketPlacePlayer(CreateMarketPlacePlayerCommand createNewMarketPlacePlayerCommand);
    }
}
