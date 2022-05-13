using FantasyTeams.Models;
using MediatR;

namespace FantasyTeams.Commands
{
    public class CreateTeamPlayerCommand : IRequest<CommandResponse>
    {
        public string TeamId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string PlayerType { get; set; }
        public double Value{ get; set; }
    }
}
