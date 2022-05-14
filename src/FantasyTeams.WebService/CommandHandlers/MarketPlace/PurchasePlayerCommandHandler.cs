using FantasyTeams.Commands.MarketPlace;
using FantasyTeams.Contracts;
using FantasyTeams.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FantasyTeams.CommandHandlers.MarketPlace
{
    public class PurchasePlayerCommandHandler : IRequestHandler<PurchasePlayerCommand, CommandResponse>
    {
        private readonly IMarketPlaceService _marketPlaceService;
        public PurchasePlayerCommandHandler(IMarketPlaceService marketPlaceService)
        {
            _marketPlaceService = marketPlaceService;
        }

        public async Task<CommandResponse> Handle(PurchasePlayerCommand request, CancellationToken cancellationToken)
        {
            return await _marketPlaceService.PurchasePlayer(request);
        }
    }
}
