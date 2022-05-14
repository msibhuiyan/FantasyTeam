using FantasyTeams.Commands;
using FantasyTeams.Commands.Player;
using FantasyTeams.Entities;
using FantasyTeams.Models;
using FantasyTeams.Queries;
using FantasyTeams.Queries.Player;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FantasyTeams.Contracts
{
    public interface IPlayerService
    {
        Task<CommandResponse> CreateNewPlayer(CreateNewPlayerCommand createNewPlayerCommand);
        Task<List<Player>> CreateNewTeamPlayers(Team teamId);
        Task<QueryResponse> GetAllPlayer(GetAllPlayerQuery getAllPlayerQuery);
        Task<CommandResponse> SetPlayerForSale(SetPlayerForSaleCommand setPlayerForSaleCommand);
        Task<CommandResponse> UpdatePlayerInfo(UpdatePlayerCommand updatePlayerCommand);
        Task<QueryResponse> GetPlayer(GetPlayerQuery request);
        Task<CommandResponse> DeletePlayer(DeletePlayerCommand deletePlayerCommand);
        Task<CommandResponse> UpdatePlayerValue(UpdatePlayerValueCommand updatePlayerPriceCommand);
        Task<CommandResponse> DeleteTeamPlayers(string teamId);
    }
}
