using FantasyTeams.Models;
using MediatR;

namespace FantasyTeams.Commands
{
    public class UserRegistrationCommand : IRequest<CommandResponse>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
