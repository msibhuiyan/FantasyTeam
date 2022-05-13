using FantasyTeams.Contracts;
using FantasyTeams.Models;
using FantasyTeams.Queries.Player;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FantasyTeams.QueryHandler.Player
{
    public class GetPlayerQueryHandler : IRequestHandler<GetPlayerQuery, QueryResponse>
    {
        private readonly IPlayerService _playerService;
        public GetPlayerQueryHandler(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        public async Task<QueryResponse> Handle(GetPlayerQuery request, CancellationToken cancellationToken)
        {
            return await _playerService.GetPlayer(request);
        }
    }
}
