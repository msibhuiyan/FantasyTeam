using FantasyTeams.Commands.Team;
using FantasyTeams.Contracts;
using FantasyTeams.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FantasyTeams.CommandHandlers.Team
{
    public class CreateTeamPlayerCommandHandler : IRequestHandler<CreateTeamPlayerCommand, CommandResponse>
    {
        private readonly ITeamService _teamService;
        public CreateTeamPlayerCommandHandler(ITeamService teamService)
        {
            _teamService = teamService;
        }

        public async Task<CommandResponse> Handle(CreateTeamPlayerCommand request, CancellationToken cancellationToken)
        {
            return await _teamService.CreateTeamPlayer(request);
        }
    }
}
