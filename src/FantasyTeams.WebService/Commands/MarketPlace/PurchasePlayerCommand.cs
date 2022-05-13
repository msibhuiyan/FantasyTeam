using FantasyTeams.Models;
using MediatR;

namespace FantasyTeams.Commands
{
    public class PurchasePlayerCommand : IRequest<CommandResponse>
    {
        public string TeamId { get; set; }
        public string PlayerId { get; set; }
    }
}
