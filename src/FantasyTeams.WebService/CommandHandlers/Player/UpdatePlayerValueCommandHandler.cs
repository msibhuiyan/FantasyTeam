using FantasyTeams.Commands;
using FantasyTeams.Contracts;
using FantasyTeams.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FantasyTeams.CommandHandlers.Player
{
    public class UpdatePlayerValueCommandHandler : IRequestHandler<UpdatePlayerValueCommand, CommandResponse>
    {
        private readonly IPlayerService _playerService;
        public UpdatePlayerValueCommandHandler(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        public async Task<CommandResponse> Handle(UpdatePlayerValueCommand request, CancellationToken cancellationToken)
        {
            return await _playerService.UpdatePlayerValue(request);
        }
    }
}
