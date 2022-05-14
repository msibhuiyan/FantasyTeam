using FantasyTeams.Models;
using MediatR;

namespace FantasyTeams.Queries.MarketPlace
{
    public class GetMarketPlacePlayerQuery : IRequest<QueryResponse>
    {
        public string PlayerId { get; set; }
    }
}
