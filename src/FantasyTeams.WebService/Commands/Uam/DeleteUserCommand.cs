using FantasyTeams.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace FantasyTeams.Commands.Uam
{
    public class DeleteUserCommand : IRequest<CommandResponse>
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
