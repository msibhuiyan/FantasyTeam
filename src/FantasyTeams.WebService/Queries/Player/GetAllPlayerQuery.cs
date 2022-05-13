using FantasyTeams.Models;
using MediatR;

namespace FantasyTeams.Queries.Player
{
    public class GetAllPlayerQuery : IRequest<QueryResponse>
    {
        public string TeamId { get; set; }
    }
}
