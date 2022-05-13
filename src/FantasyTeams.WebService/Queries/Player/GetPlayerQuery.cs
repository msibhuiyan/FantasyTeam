using FantasyTeams.Models;
using MediatR;

namespace FantasyTeams.Queries
{
    public class GetPlayerQuery : IRequest<QueryResponse>
    {
        public string PlayerId { get; set; }
    }
}
