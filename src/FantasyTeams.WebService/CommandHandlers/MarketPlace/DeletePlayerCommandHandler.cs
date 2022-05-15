using FantasyTeams.Commands.MarketPlace;
using FantasyTeams.Contracts;
using FantasyTeams.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FantasyTeams.CommandHandlers.MarketPlace
{
    public class DeletePlayerCommandHandler : IRequestHandler<DeleteMarketPlacePlayerCommand, CommandResponse>
    {
        private readonly IMarketPlaceService _marketplaceService;
        public DeletePlayerCommandHandler(IMarketPlaceService marketplaceService)
        {
            _marketplaceService = marketplaceService;
        }

        public async Task<CommandResponse> Handle(DeleteMarketPlacePlayerCommand request, CancellationToken cancellationToken)
        {
            return await _marketplaceService.DeletePlayer(request);
        }
    }
}
