using FantasyTeams.Models;
using MediatR;

namespace FantasyTeams.Commands
{
    public class CreateNewPlayerCommand : IRequest<CommandResponse>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
    }
}
