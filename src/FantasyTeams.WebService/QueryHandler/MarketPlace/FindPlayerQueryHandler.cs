using FantasyTeams.Contracts;
using FantasyTeams.Models;
using FantasyTeams.Queries;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FantasyTeams.QueryHandler.MarketPlace
{
    public class FindPlayerQueryHandler : IRequestHandler<FindPlayerQuery, QueryResponse>
    {
        private readonly IMarketPlaceService _marketPlaceService;
        public FindPlayerQueryHandler(IMarketPlaceService marketPlaceService)
        {
            _marketPlaceService = marketPlaceService;
        }

        public async Task<QueryResponse> Handle(FindPlayerQuery request, CancellationToken cancellationToken)
        {
            return await _marketPlaceService.FindMarketPlacePlayer(request);
        }
    }
}
