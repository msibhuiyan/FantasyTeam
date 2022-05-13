using FantasyTeams.Models;
using MediatR;

namespace FantasyTeams.Commands
{
    public class UpdatePlayerValueCommand : IRequest<CommandResponse>
    {
        public string PlayerId { get; set; }
        public double PlayerValue { get; set; }
    }
}
