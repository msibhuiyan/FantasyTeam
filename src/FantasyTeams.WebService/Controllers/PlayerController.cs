using FantasyTeams.Commands;
using FantasyTeams.Contracts;
using FantasyTeams.Entities;
using FantasyTeams.Models;
using FantasyTeams.Queries;
using FantasyTeams.Repository;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FantasyTeams.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly ILogger<PlayerController> _logger;
        private readonly IMediator _mediator;
        public PlayerController(ILogger<PlayerController> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("CreatePlayer")]
        public async Task<CommandResponse> CreatePlayer([FromBody] CreateNewPlayerCommand createNewPlayerCommand)
        {
            return await _mediator.Send(createNewPlayerCommand);
        }
        [Authorize(Roles = "Admin, Member")]
        [HttpGet("GetAllPlayer")]
        public async Task<QueryResponse> GetAllPlayer()
        {
            return await _mediator.Send(new GetAllPlayerQuery());
        }
        [Authorize(Roles = "Member")]
        [HttpPost("SetForSale")]
        public async Task<CommandResponse> MoveToMarketPlace([FromBody] SetPlayerForSaleCommand moveToMarketPlaceCommand)
        {
            return await _mediator.Send(moveToMarketPlaceCommand);
        }
        [Authorize(Roles = "Member")]
        [HttpPut("UpdatePlayer")]
        public async Task<CommandResponse> UpdatePlayer([FromBody] UpdatePlayerCommand updatePlayerCommand)
        {
            return await _mediator.Send(updatePlayerCommand);
        }
        [Authorize(Roles = "Admin, Member")]
        [HttpPut("UpdatePlayerPrice")]
        public async Task<CommandResponse> UpdatePlayerPrice([FromBody] UpdatePlayerValueCommand updatePlayerPriceCommand)
        {
            return await _mediator.Send(updatePlayerPriceCommand);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("DeletePlayer")]
        public async Task<CommandResponse> DeletePlayer([FromBody] DeletePlayerCommand deletePlayerCommand)
        {
            return await _mediator.Send(deletePlayerCommand);
        }
    }
}
