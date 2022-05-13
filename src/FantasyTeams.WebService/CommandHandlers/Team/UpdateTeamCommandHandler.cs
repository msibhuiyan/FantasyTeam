using FantasyTeams.Commands;
using FantasyTeams.Contracts;
using FantasyTeams.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FantasyTeams.CommandHandlers.Team
{
    public class UpdateTeamCommandHandler : IRequestHandler<UpdateTeamCommand, CommandResponse>
    {
        private readonly ITeamService _teamService;
        public UpdateTeamCommandHandler(ITeamService teamService)
        {
            _teamService = teamService;
        }
        public Task<CommandResponse> Handle(UpdateTeamCommand request, CancellationToken cancellationToken)
        {
            return _teamService.UpdateTeamInfo(request);
        }
    }
}
