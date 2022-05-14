using FantasyTeams.Contracts;
using FantasyTeams.Models;
using FantasyTeams.Queries.MarketPlace;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FantasyTeams.QueryHandler.MarketPlace
{
    public class GetMarketPlacePlayerQueryHandler : IRequestHandler<GetMarketPlacePlayerQuery, QueryResponse>
    {
        private readonly IMarketPlaceService _marketPlaceService;
        public GetMarketPlacePlayerQueryHandler(IMarketPlaceService marketPlaceService)
        {
            _marketPlaceService = marketPlaceService;
        }

        public async Task<QueryResponse> Handle(GetMarketPlacePlayerQuery request, CancellationToken cancellationToken)
        {
            return await _marketPlaceService.GetMarketPlacePlayer(request.PlayerId);
        }
    }
}
