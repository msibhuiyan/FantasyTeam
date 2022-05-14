using FantasyTeams.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FantasyTeams.Commands
{
    public class UpdatePlayerCommand : IRequest<CommandResponse>
    {
        [Required]
        [RegularExpression("[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}",
            ErrorMessage = "Please provide correct GUID")]
        public string PlayerId { get; set; }
        [RegularExpression("[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}",
            ErrorMessage = "Please provide correct GUID")]
        [JsonIgnore]
        public string TeamId
        {
            get; set;
        }
        public string Country { get; set; }
        [RegularExpression("[a-zA-Z0-9]+",
            ErrorMessage = "Please provide alpha numeric value")]
        public string FirstName { get; set; }
        [RegularExpression("[a-zA-Z0-9]+",
            ErrorMessage = "Please provide alpha numeric value")]
        public string LastName { get; set; }
    }
}
