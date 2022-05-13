using FantasyTeams.Enums;
using FantasyTeams.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace FantasyTeams.Commands
{
    public class CreateMarketPlacePlayerCommand : IRequest<CommandResponse>
    {
        [Required]
        [RegularExpression("^[a-zA-Z0-9]?$",
            ErrorMessage = "Please provide alpha numeric value")]
        public string FirstName { get; set; }
        [Required]
        [RegularExpression("^[a-zA-Z0-9]?$",
            ErrorMessage = "Please provide alpha numeric value")]
        public string LastName { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        [Range(0, 1000000000, ErrorMessage = "Please provide Asking price between 0 to 1000000000")]
        public double AskingPrice { get; set; }
        [Required]
        public string PlayerType { get; set; }
    }
}
