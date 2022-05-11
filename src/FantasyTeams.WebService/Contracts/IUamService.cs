﻿using FantasyTeams.Commands;
using FantasyTeams.Entities;
using FantasyTeams.Models;
using System.Threading.Tasks;

namespace FantasyTeams.Contracts
{
    public interface IUamService
    {
        Task<CommandResponse> RegisterUser(UserRegistrationCommand userRegistrationCommand);
        Task<AuthCommandResponse> UserLogin(UserLoginCommand userLoginCommand);
        Task<User> GetUserInfo(string userEmail);
        Task<CommandResponse> DeleteUser(DeleteUserCommand deleteUserCommand);
    }
}
