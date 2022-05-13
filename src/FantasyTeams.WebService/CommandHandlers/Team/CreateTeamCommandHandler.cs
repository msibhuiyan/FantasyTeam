using FantasyTeams.Commands;
using FantasyTeams.Contracts;
using FantasyTeams.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FantasyTeams.CommandHandlers.Team
{
    public class CreateTeamCommandHandler : IRequestHandler<CreateTeamCommand, CommandResponse>
    {
        private readonly ITeamService _teamService;
        public CreateTeamCommandHandler(ITeamService teamService)
        {
            _teamService = teamService;
        }

        public async Task<CommandResponse> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
        {
            return await _teamService.CreateNewTeam(request);
        }

    }
}
