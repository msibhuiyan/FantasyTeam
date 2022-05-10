using FantasyTeams.Commands;
using FantasyTeams.Entities;
using System.Threading.Tasks;

namespace FantasyTeams.Contracts
{
    public interface IUamService
    {
        Task RegisterUser(UserRegistrationCommand userRegistrationCommand);
        Task<string> UserLogin(UserLoginCommand userLoginCommand);
        Task<User> GetUserInfo(string userEmail);
        Task DeleteUser(DeleteUserCommand deleteUserCommand);
    }
}
