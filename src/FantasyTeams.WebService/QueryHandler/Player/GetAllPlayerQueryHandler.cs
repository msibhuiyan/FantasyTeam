using FantasyTeams.Contracts;
using FantasyTeams.Models;
using FantasyTeams.Queries;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FantasyTeams.QueryHandler.Player
{
    public class GetAllPlayerQueryHandler : IRequestHandler<GetAllPlayerQuery, QueryResponse>
    {
        private readonly IPlayerService _playerService;
        public GetAllPlayerQueryHandler(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        public async Task<QueryResponse> Handle(GetAllPlayerQuery request, CancellationToken cancellationToken)
        {
            return await _playerService.GetAllPlayer(request);
        }
    }
}
