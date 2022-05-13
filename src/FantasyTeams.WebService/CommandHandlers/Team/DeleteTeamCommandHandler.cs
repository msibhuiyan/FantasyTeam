using FantasyTeams.Commands;
using FantasyTeams.Contracts;
using FantasyTeams.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FantasyTeams.CommandHandlers.Team
{
    public class DeleteTeamCommandHandler : IRequestHandler<DeleteTeamCommand, CommandResponse>
    {
        private readonly ITeamService _teamService;
        public DeleteTeamCommandHandler(ITeamService teamService)
        {
            _teamService = teamService;
        }

        public async Task<CommandResponse> Handle(DeleteTeamCommand request, CancellationToken cancellationToken)
        {
            return await _teamService.DeleteTeam(request);
        }
    }
}
