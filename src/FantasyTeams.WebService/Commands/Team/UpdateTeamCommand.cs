using FantasyTeams.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace FantasyTeams.Commands
{
    public class UpdateTeamCommand : IRequest<CommandResponse>
    {
        [Required]
        public string TeamId { get; set; }
        public string Country { get; set; }
        public string Name { get; set; }
    }
}
