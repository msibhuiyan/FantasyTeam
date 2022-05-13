using FantasyTeams.Models;
using MediatR;

namespace FantasyTeams.Commands
{
    public class UpdatePlayerCommand : IRequest<CommandResponse>
    {
        public string PlayerId { get; set; }
        public string Country { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
