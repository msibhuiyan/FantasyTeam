using FantasyTeams.Contracts;
using FantasyTeams.Models;
using FantasyTeams.Queries;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FantasyTeams.QueryHandler.Team
{
    public class GetTeamQueryHandler : IRequestHandler<GetTeamQuery, QueryResponse>
    {
        private readonly ITeamService _teamService;
        public GetTeamQueryHandler(ITeamService teamService)
        {
            _teamService = teamService;
        }

        public async Task<QueryResponse> Handle(GetTeamQuery request, CancellationToken cancellationToken)
        {
            return await _teamService.GetTeamInfo(request);
        }
    }
}
