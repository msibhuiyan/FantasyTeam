using FantasyTeams.Contracts;
using FantasyTeams.Models;
using FantasyTeams.Queries;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FantasyTeams.QueryHandler.Team
{
    public class GetAllTeamQueryHandler : IRequestHandler<GetAllTeamQuery, QueryResponse>
    {
        private readonly ITeamService _teamService;
        public GetAllTeamQueryHandler(ITeamService teamService)
        {
            _teamService = teamService;
        }

        public async Task<QueryResponse> Handle(GetAllTeamQuery request, CancellationToken cancellationToken)
        {
            return await _teamService.GetAllTeams();
        }
    }
}
