using FantasyTeams.Commands;
using FantasyTeams.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FantasyTeams.Contracts
{
    public interface IPlayerService
    {
        Task CreateNewPlayer(CreateNewPlayerCommand createNewPlayerCommand);
        Task<List<Player>> CreateNewTeamPlayers(string teamId);
        Task<List<Player>> GetAllPlayer();
        Task SetPlayerForSale(SetPlayerForSaleCommand setPlayerForSaleCommand);
        Task UpdatePlayerInfo(UpdatePlayerCommand updatePlayerCommand);
        Task DeletePlayer(DeletePlayerCommand deletePlayerCommand);
        Task UpdatePlayerValue(UpdatePlayerPriceCommand updatePlayerPriceCommand);
        Task DeleteTeamPlayers(string teamId);
    }
}
