using FantasyTeams.Commands;
using FantasyTeams.Entities;
using FantasyTeams.Models;
using FantasyTeams.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FantasyTeams.Contracts
{
    public interface IPlayerService
    {
        Task<CommandResponse> CreateNewPlayer(CreateNewPlayerCommand createNewPlayerCommand);
        Task<List<Player>> CreateNewTeamPlayers(string teamId);
        Task<QueryResponse> GetAllPlayer(GetAllPlayerQuery getAllPlayerQuery);
        Task<CommandResponse> SetPlayerForSale(SetPlayerForSaleCommand setPlayerForSaleCommand);
        Task<CommandResponse> UpdatePlayerInfo(UpdatePlayerCommand updatePlayerCommand);
        Task<CommandResponse> DeletePlayer(DeletePlayerCommand deletePlayerCommand);
        Task<CommandResponse> UpdatePlayerValue(UpdatePlayerValueCommand updatePlayerPriceCommand);
        Task<CommandResponse> DeleteTeamPlayers(string teamId);
    }
}
