using FantasyTeams.Models;
using MediatR;

namespace FantasyTeams.Commands.Team
{
    public class AssignTeamCommand : IRequest<CommandResponse>
    {
        public string UserId { get; set; }
        public string TeamId { get; set; }
    }
}
