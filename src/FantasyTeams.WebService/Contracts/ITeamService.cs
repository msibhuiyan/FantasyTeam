using FantasyTeams.Commands;
using System.Threading.Tasks;

namespace FantasyTeams.Contracts
{
    public interface ITeamService
    {
        Task CreateNewTeam(CreateNewTeamCommand createNewTeamCommand);
    }
}
