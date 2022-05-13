using FantasyTeams.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace FantasyTeams.Commands
{
    public class SetPlayerForSaleCommand : IRequest<CommandResponse>
    {
        [Required]
        [RegularExpression("[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}",
            ErrorMessage = "Please provide correct GUID")]
        public string PlayerId { get; set; }
        [Required]
        [Range(0, 1000000000, ErrorMessage = "Please provide Asking price between 0 to 1000000000")]
        public double AskingPrice { get; set; }
    }
}
