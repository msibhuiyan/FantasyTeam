using FantasyTeams.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace FantasyTeams.Commands
{
    public class DeleteUserCommand : IRequest<CommandResponse>
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
