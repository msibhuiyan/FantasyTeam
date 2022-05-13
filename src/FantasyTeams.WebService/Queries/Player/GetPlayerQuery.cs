using FantasyTeams.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FantasyTeams.Queries.Player
{
    public class GetPlayerQuery : IRequest<QueryResponse>
    {
        [Required]
        public string PlayerId { get; set; }
        [JsonIgnore]
        public string TeamId { get; set; }
    }
}
