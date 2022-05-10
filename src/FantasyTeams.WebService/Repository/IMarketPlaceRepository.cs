using FantasyTeams.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FantasyTeams.Repository
{
    public interface IMarketPlaceRepository
    {
        Task<List<Player>> GetAllAsync();
        Task CreateAsync(Player player);
        Task UpdateAsync(string id, Player team);
        Task<Player> GetByIdAsync(string id);
        Task<List<Player>> GetPlayer(string playerName, string teamName, string country, double value);
    }
}
