using FantasyTeams.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace FantasyTeams.Commands
{
    public class UpdateTeamCommand : IRequest<CommandResponse>
    {
        [Required]
        [RegularExpression("^[{]?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}[}]?$",
            ErrorMessage = "Please provide correct GUID")]
        public string TeamId { get; set; }
        [RegularExpression("^[a-zA-Z]?$",
            ErrorMessage = "Please provide alphabetic value")]
        public string Country { get; set; }
        [RegularExpression("^[a-zA-Z]?$",
            ErrorMessage = "Please provide alpha numeric value")]
        public string Name { get; set; }
    }
}
