using FantasyTeams.Commands;
using FantasyTeams.Queries;
using FantasyTeams.Contracts;
using FantasyTeams.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using FantasyTeams.Models;

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
        [HttpGet("GetAllPlayer")]
        public async Task<QueryResponse> GetAllPlayer()
        {
            return await _mediator.Send(new GetAllPlayerQuery());
        }
        [Authorize(Roles = "Admin, Member")]
        [HttpGet("GetPlayer")]
        public async Task<QueryResponse> GetPlayer([FromQuery] string PlayerId)
        {
            return await _mediator.Send(
                new GetPlayerQuery
                { 
                    PlayerId = PlayerId 
                });
        }
        [Authorize(Roles = "Admin, Member")]
        [HttpPost("FindPlayer")]
        public async Task<QueryResponse> FindPlayer(
            [FromBody] FindPlayerQuery findPlayerQuery)
        {
            return await _mediator.Send(findPlayerQuery);
        }
        [Authorize(Roles = "Member")]
        [HttpPost("PurchasePlayer")]
        public async Task<CommandResponse> PurchasePlayer([FromBody] PurchasePlayerCommand purchasePlayerCommand)
        {
            return await _mediator.Send(purchasePlayerCommand);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("DeletePlayer")]
        public async Task<CommandResponse> DeletePlayer([FromBody] DeletePlayerCommand deletePlayerCommand)
        {
            return await _mediator.Send(deletePlayerCommand);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("CreatePlayer")]
        public async Task<CommandResponse> CreateNewMarketPlacePlayer([FromBody] 
        CreateMarketPlacePlayerCommand createNewMarketPlacePlayerCommand)
        {
            return await _mediator.Send(createNewMarketPlacePlayerCommand);
        }
    }
}
