using FantasyTeams.Commands.Uam;
using FantasyTeams.Entities;
using FantasyTeams.Models;
using FantasyTeams.Queries.Uam;
using System.Threading.Tasks;

namespace FantasyTeams.Contracts
{
    public interface IUamService
    {
        Task<CommandResponse> RegisterUser(UserRegistrationCommand userRegistrationCommand);
        Task<CommandResponse> UserLogin(UserLoginCommand userLoginCommand);
        Task<CommandResponse> DeleteUser(DeleteUserCommand deleteUserCommand);
        Task<CommandResponse> OnboardUser(OnboardUserCommand request);
        Task<QueryResponse> GetUnAssignedUser(GetUnAssignedUserQuery request);
    }
}
