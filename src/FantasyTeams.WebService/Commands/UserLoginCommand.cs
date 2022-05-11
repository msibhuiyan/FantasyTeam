using FantasyTeams.Models;
using MediatR;

namespace FantasyTeams.Commands
{
    public class UserLoginCommand : IRequest<AuthCommandResponse>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
