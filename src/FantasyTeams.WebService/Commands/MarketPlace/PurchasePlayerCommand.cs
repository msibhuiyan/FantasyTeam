using FantasyTeams.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace FantasyTeams.Commands
{
    public class PurchasePlayerCommand : IRequest<CommandResponse>
    {
        [Required]
        [RegularExpression("[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}",
            ErrorMessage = "Please provide correct GUID")]
        public string TeamId { get; set; }
        [Required]
        [RegularExpression("[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}",
            ErrorMessage = "Please provide correct GUID")]
        public string PlayerId { get; set; }
    }
}
