using FantasyTeams.Commands;
using FantasyTeams.Contracts;
using FantasyTeams.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FantasyTeams.CommandHandlers.MarketPlace
{
    public class CreateMarketPlacePlayerCommandHandler : IRequestHandler<CreateMarketPlacePlayerCommand, CommandResponse>
    {
        private readonly IMarketPlaceService _marketPlaceService;
        public CreateMarketPlacePlayerCommandHandler(IMarketPlaceService marketPlaceService)
        {
            _marketPlaceService = marketPlaceService;
        }

        public async Task<CommandResponse> Handle(CreateMarketPlacePlayerCommand request, CancellationToken cancellationToken)
        {
            return await _marketPlaceService.CreateNewMarketPlacePlayer(request);
        }
    }
}
