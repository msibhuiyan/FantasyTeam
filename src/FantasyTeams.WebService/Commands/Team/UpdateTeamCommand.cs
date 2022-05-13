using FantasyTeams.Models;
using MediatR;

namespace FantasyTeams.Commands
{
    public class UpdateTeamCommand : IRequest<CommandResponse>
    {
        public string TeamId { get; set; }
        public string Country { get; set; }
        public string Name { get; set; }
    }
}
