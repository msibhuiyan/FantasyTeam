using FantasyTeams.Models;
using MediatR;
using System.Threading.Tasks;

namespace FantasyTeams.Commands
{
    public class DeleteUserCommand : IRequest<CommandResponse>
    {
        public string Email { get; set; }
    }
}
