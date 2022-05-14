using FantasyTeams.Commands;
using FantasyTeams.Commands.Player;
using FantasyTeams.Contracts;
using FantasyTeams.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FantasyTeams.CommandHandlers.Player
{
    public class SetPlayerForSaleCommandHandler : IRequestHandler<SetPlayerForSaleCommand, CommandResponse>
    {
        private readonly IPlayerService _playerService;
        public SetPlayerForSaleCommandHandler(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        public async Task<CommandResponse> Handle(SetPlayerForSaleCommand request, CancellationToken cancellationToken)
        {
            return await _playerService.SetPlayerForSale(request);
        }
    }
}
