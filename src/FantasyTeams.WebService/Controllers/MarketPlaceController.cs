using FantasyTeams.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using MediatR;
using FantasyTeams.Models;
using FantasyTeams.Queries.MarketPlace;
using System.Security.Claims;
using FantasyTeams.Commands.MarketPlace;

namespace FantasyTeams.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MarketPlaceController : ControllerBase
    {
        private readonly ILogger<MarketPlaceController> _logger;
        private readonly IMediator _mediator;
        public MarketPlaceController(ILogger<MarketPlaceController> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        [Authorize(Roles = "Admin, Member")]
        [HttpGet("Players")]
        public async Task<QueryResponse> GetAllPlayer()
        {
            return await _mediator.Send(new GetAllPlayerQuery());
        }
        [Authorize(Roles = "Admin, Member")]
        [HttpGet("PlayerInfo")]
        public async Task<QueryResponse> GetPlayer([FromQuery] string PlayerId)
        {
            return await _mediator.Send(
                new GetMarketPlacePlayerQuery
                { 
                    PlayerId = PlayerId 
                });
        }
        [Authorize(Roles = "Admin, Member")]
        [HttpGet("Search")]
        public async Task<QueryResponse> FindPlayer(
            [FromBody] FindPlayerQuery findPlayerQuery)
        {
            return await _mediator.Send(findPlayerQuery);
        }
        [Authorize(Roles = "Member")]
        [HttpPost("Purchase")]
        public async Task<CommandResponse> PurchasePlayer([FromBody] PurchasePlayerCommand purchasePlayerCommand)
        {
            var teamId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            purchasePlayerCommand.TeamId = teamId;
            return await _mediator.Send(purchasePlayerCommand);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete")]
        public async Task<CommandResponse> DeletePlayer([FromBody] DeleteMarketPlacePlayerCommand deletePlayerCommand)
        {
            return await _mediator.Send(deletePlayerCommand);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Create")]
        public async Task<CommandResponse> CreateNewMarketPlacePlayer([FromBody] 
        CreateMarketPlacePlayerCommand createNewMarketPlacePlayerCommand)
        {
            return await _mediator.Send(createNewMarketPlacePlayerCommand);
        }
    }
}
