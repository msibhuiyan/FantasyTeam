using FantasyTeams.Commands;
using FantasyTeams.Contracts;
using FantasyTeams.Entities;
using FantasyTeams.Repository;
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
        public PlayerController(ILogger<PlayerController> logger,
            IPlayerService playerService)
        {
            _logger = logger;
            _playerService = playerService;
        }

        [HttpPost("CreatePlayer")]
        public async Task CreatePlayer([FromBody] CreateNewPlayerCommand createNewPlayerCommand)
        {
            await _playerService.CreateNewPlayer(createNewPlayerCommand);
        }
        [HttpGet("GetAllPlayer")]
        public async Task<List<Player>> GetAllPlayer()
        {
            return await _playerService.GetAllPlayer();
        }

        [HttpPost("SetForSale")]
        public async Task<Player> MoveToMarketPlace([FromBody] SetPlayerForSaleCommand moveToMarketPlaceCommand)
        {
            await _playerService.SetPlayerForSale(moveToMarketPlaceCommand);
            return null;
        }

        [HttpPut("UpdatePlayer")]
        public async Task UpdatePlayer([FromBody] UpdatePlayerCommand updatePlayerCommand)
        {
            await _playerService.UpdatePlayerInfo(updatePlayerCommand);
        }

        [HttpPut("UpdatePlayerPrice")]
        public async Task UpdatePlayerPrice([FromBody] UpdatePlayerPriceCommand updatePlayerPriceCommand)
        {
            await _playerService.UpdatePlayerValue(updatePlayerPriceCommand);
        }

        [HttpDelete("DeletePlayer")]
        public async Task DeletePlayer([FromBody] DeletePlayerCommand deletePlayerCommand)
        {
            await _playerService.DeletePlayer(deletePlayerCommand);
        }
    }
}
