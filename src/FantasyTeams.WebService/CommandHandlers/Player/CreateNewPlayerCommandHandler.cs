using FantasyTeams.Commands;
using FantasyTeams.Commands.Player;
using FantasyTeams.Contracts;
using FantasyTeams.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FantasyTeams.CommandHandlers.Player
{
    public class CreateNewPlayerCommandHandler : IRequestHandler<CreateNewPlayerCommand, CommandResponse>
    {
        private readonly IPlayerService _playerService;
        public CreateNewPlayerCommandHandler(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        public async Task<CommandResponse> Handle(CreateNewPlayerCommand request, CancellationToken cancellationToken)
        {
            return await _playerService.CreateNewPlayer(request);
        }
    }
}
