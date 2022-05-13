using FantasyTeams.Models;
using MediatR;

namespace FantasyTeams.Commands
{
    public class DeletePlayerCommand : IRequest<CommandResponse>
    {
        public string PlayerId { get; set; }
    }
}
