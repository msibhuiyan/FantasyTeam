using FantasyTeams.Commands;
using FantasyTeams.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FantasyTeams.Contracts
{
    public interface IMarketPlaceService
    {
        Task<List<Player>> GetAllMarketPlacePlayer();
        Task<Player> GetMarketPlacePlayer(string PlayerId);
        Task PurchasePlayer(PurchasePlayerCommand purchasePlayerCommand);
    }
}
