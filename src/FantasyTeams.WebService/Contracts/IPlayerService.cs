using FantasyTeams.Commands;
using FantasyTeams.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FantasyTeams.Contracts
{
    public interface IPlayerService
    {
        Task CreateNewPlayer(CreateNewPlayerCommand createNewPlayerCommand);
        Task<List<Player>> GetAllPlayer();
    }
}
