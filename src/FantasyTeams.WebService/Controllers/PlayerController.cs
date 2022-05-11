using FantasyTeams.Commands;
using FantasyTeams.Contracts;
using FantasyTeams.Entities;
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
        private readonly IPlayerService _playerService;
        private readonly IMediator _mediator;
        public PlayerController(ILogger<PlayerController> logger,
            IPlayerService playerService,
            IMediator mediator)
        {
            _logger = logger;
            _playerService = playerService;
            _mediator = mediator;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("CreatePlayer")]
        public async Task CreatePlayer([FromBody] CreateNewPlayerCommand createNewPlayerCommand)
        {
            await _playerService.CreateNewPlayer(createNewPlayerCommand);
        }
        [Authorize(Roles = "Admin, Member")]
        [HttpGet("GetAllPlayer")]
        public async Task<List<Player>> GetAllPlayer()
        {
            return await _playerService.GetAllPlayer();
        }
        [Authorize(Roles = "Member")]
        [HttpPost("SetForSale")]
        public async Task<Player> MoveToMarketPlace([FromBody] SetPlayerForSaleCommand moveToMarketPlaceCommand)
        {
            await _playerService.SetPlayerForSale(moveToMarketPlaceCommand);
            return null;
        }
        [Authorize(Roles = "Member")]
        [HttpPut("UpdatePlayer")]
        public async Task UpdatePlayer([FromBody] UpdatePlayerCommand updatePlayerCommand)
        {
            await _playerService.UpdatePlayerInfo(updatePlayerCommand);
        }
        [Authorize(Roles = "Admin, Member")]
        [HttpPut("UpdatePlayerPrice")]
        public async Task UpdatePlayerPrice([FromBody] UpdatePlayerPriceCommand updatePlayerPriceCommand)
        {
            await _playerService.UpdatePlayerValue(updatePlayerPriceCommand);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("DeletePlayer")]
        public async Task DeletePlayer([FromBody] DeletePlayerCommand deletePlayerCommand)
        {
            await _playerService.DeletePlayer(deletePlayerCommand);
        }
    }
}
