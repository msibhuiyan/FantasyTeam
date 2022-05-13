using FantasyTeams.Contracts;
using FantasyTeams.Models;
using FantasyTeams.Queries.MarketPlace;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FantasyTeams.QueryHandler.MarketPlace
{
    public class GetAllPlayerQueryHandler : IRequestHandler<GetAllPlayerQuery, QueryResponse>
    {
        private readonly IMarketPlaceService _marketPlaceService;
        public GetAllPlayerQueryHandler(IMarketPlaceService marketPlaceService)
        {
            _marketPlaceService = marketPlaceService;
        }

        public async Task<QueryResponse> Handle(GetAllPlayerQuery request, CancellationToken cancellationToken)
        {
            return await _marketPlaceService.GetAllMarketPlacePlayer();
        }
    }
}
