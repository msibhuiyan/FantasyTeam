using FantasyTeams.Models;
using MediatR;

namespace FantasyTeams.Queries
{
    public class GetTeamQuery : IRequest<QueryResponse>
    {
        public string TeamId { get; set; }
        public string TeamName { get; set; }
    }
}
