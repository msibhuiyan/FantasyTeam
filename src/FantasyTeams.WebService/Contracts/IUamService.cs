using FantasyTeams.Commands.Uam;
using FantasyTeams.Entities;
using FantasyTeams.Models;
using System.Threading.Tasks;

namespace FantasyTeams.Contracts
{
    public interface IUamService
    {
        Task<CommandResponse> RegisterUser(UserRegistrationCommand userRegistrationCommand);
        Task<CommandResponse> UserLogin(UserLoginCommand userLoginCommand);
        Task<User> GetUserInfo(string userEmail);
        Task<CommandResponse> DeleteUser(DeleteUserCommand deleteUserCommand);
        Task<CommandResponse> OnboardUser(OnboardUserCommand request);
    }
}
