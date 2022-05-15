using FantasyTeams.Commands.Team;
using FantasyTeams.Contracts;
using FantasyTeams.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
namespace FantasyTeams.CommandHandlers.Team
{
    public class AssignTeamCommandHandler : IRequestHandler<AssignTeamCommand, CommandResponse>
    {
        private readonly ITeamService _teamService;
        public AssignTeamCommandHandler(ITeamService teamService)
        {
            _teamService = teamService;
        }

        public async Task<CommandResponse> Handle(AssignTeamCommand request, CancellationToken cancellationToken)
        {
            return await _teamService.AssignToUser(request);
        }
    {
    }
}
