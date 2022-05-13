using FantasyTeams.Models;
using MediatR;

namespace FantasyTeams.Queries
{
    public class FindPlayerQuery : IRequest<QueryResponse>
    {
        public string PlayerName { get; set; }
        public string TeamName { get; set; }
        public string Country { get; set; }
        public double Value { get; set; }
    }
}
