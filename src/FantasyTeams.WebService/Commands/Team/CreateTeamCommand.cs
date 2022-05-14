using FantasyTeams.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace FantasyTeams.Commands.Team
{
    public class CreateTeamCommand : IRequest<CommandResponse>
    {
        [Required]
        [RegularExpression("[a-zA-Z0-9]+",
            ErrorMessage = "Please provide alpha numeric value")]
        public string Name { get; set; }
        [Required]
        public string Country { get; set; }
    }
}
