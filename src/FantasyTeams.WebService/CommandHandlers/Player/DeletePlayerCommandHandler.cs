using FantasyTeams.Commands;
using FantasyTeams.Contracts;
using FantasyTeams.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FantasyTeams.CommandHandlers.Player
{
    public class DeletePlayerCommandHandler : IRequestHandler<DeletePlayerCommand, CommandResponse>
    {
        private readonly IPlayerService _playerService;
        public DeletePlayerCommandHandler(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        public async Task<CommandResponse> Handle(DeletePlayerCommand request, CancellationToken cancellationToken)
        {
            return await _playerService.DeletePlayer(request);
        }
    }
}
