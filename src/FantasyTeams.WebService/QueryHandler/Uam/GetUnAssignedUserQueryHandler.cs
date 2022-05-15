using FantasyTeams.Contracts;
using FantasyTeams.Models;
using FantasyTeams.Queries.Uam;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FantasyTeams.QueryHandler.Uam
{
    public class GetUnAssignedUserQueryHandler : IRequestHandler<GetUnAssignedUserQuery, QueryResponse>
    {
        private readonly IUamService _userService;
        public GetUnAssignedUserQueryHandler(IUamService userService)
        {
            _userService = userService;
        }

        public async Task<QueryResponse> Handle(GetUnAssignedUserQuery request, CancellationToken cancellationToken)
        {
            return await _userService.GetUnAssignedUser(request);
        }
    }
}
