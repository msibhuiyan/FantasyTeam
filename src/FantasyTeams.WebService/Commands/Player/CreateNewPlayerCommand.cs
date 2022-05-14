using FantasyTeams.Enums;
using FantasyTeams.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace FantasyTeams.Commands.Player
{
    public class CreateNewPlayerCommand : IRequest<CommandResponse>
    {
        [Required]
        [RegularExpression("[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}",
            ErrorMessage = "Please provide correct GUID")]
        public string TeamId { get; set; }
        [Required]
        [RegularExpression("[a-zA-Z0-9]+",
            ErrorMessage = "Please provide alpha numeric value")]
        public string FirstName { get; set; }
        [Required]
        [RegularExpression("[a-zA-Z0-9]+",
            ErrorMessage = "Please provide alpha numeric value")]
        public string LastName { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        [EnumDataType(typeof(PlayerType), ErrorMessage = "Select From GoalKeeper, Attacker, Defender, MidFielder")]
        public string PlayerType { get; set; }
    }
}
