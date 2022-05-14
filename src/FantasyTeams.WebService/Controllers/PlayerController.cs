using FantasyTeams.Commands;
using FantasyTeams.Commands.Player;
using FantasyTeams.Contracts;
using FantasyTeams.Entities;
using FantasyTeams.Models;
using FantasyTeams.Queries;
using FantasyTeams.Queries.Player;
using FantasyTeams.Repository;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Security.Claims;
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
            var teamId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            string role = User.FindFirst(ClaimTypes.Role).Value;
            if(role == "Admin")
            {
                return await _mediator.Send(new GetAllPlayerQuery());
            }
            return await _mediator.Send(new GetAllPlayerQuery
            {
                TeamId = teamId
            });

        }
        [Authorize(Roles = "Admin, Member")]
        [HttpGet("GetPlayer")]
        public async Task<QueryResponse> GetPlayer([FromQuery] GetPlayerQuery getPlayerQuery)
        {
            var teamId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            getPlayerQuery.TeamId = teamId;
            return await _mediator.Send(getPlayerQuery);

        }
        [Authorize(Roles = "Member")]
        [HttpPost("SetForSale")]
        public async Task<CommandResponse> MoveToMarketPlace([FromBody] SetPlayerForSaleCommand moveToMarketPlaceCommand)
        {
            var teamId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            moveToMarketPlaceCommand.TeamId = teamId;
            return await _mediator.Send(moveToMarketPlaceCommand);
        }
        [Authorize(Roles = "Admin, Member")]
        [HttpPut("UpdatePlayer")]
        public async Task<CommandResponse> UpdatePlayer([FromBody] UpdatePlayerCommand updatePlayerCommand)
        {
            var teamId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            updatePlayerCommand.TeamId = teamId;

            return await _mediator.Send(updatePlayerCommand);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("UpdatePlayerValue")]
        public async Task<CommandResponse> UpdatePlayerValue([FromBody] UpdatePlayerValueCommand updatePlayerValueCommand)
        {
            return await _mediator.Send(updatePlayerValueCommand);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("DeletePlayer")]
        public async Task<CommandResponse> DeletePlayer([FromBody] DeletePlayerCommand deletePlayerCommand)
        {
            return await _mediator.Send(deletePlayerCommand);
        }
    }
}
