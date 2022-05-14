using FantasyTeams.Commands;
using FantasyTeams.Commands.Player;
using FantasyTeams.Contracts;
using FantasyTeams.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FantasyTeams.CommandHandlers.Player
{
    public class UpdatePlayerCommandHandler : IRequestHandler<UpdatePlayerCommand, CommandResponse>
    {
        private readonly IPlayerService _playerService;
        public UpdatePlayerCommandHandler(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        public async Task<CommandResponse> Handle(UpdatePlayerCommand request, CancellationToken cancellationToken)
        {
            return await _playerService.UpdatePlayerInfo(request);
        }
    }
}
