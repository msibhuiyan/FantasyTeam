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
        public async Task<Player> CreatePlayer([FromBody] CreateNewPlayerCommand createNewPlayerCommand)
        {
            await _playerService.CreateNewPlayer(createNewPlayerCommand);
            return null;
        }
        [HttpGet("GetAllPlayer")]
        public async Task<List<Player>> GetAllPlayer()
        {
            return await _playerService.GetAllPlayer();
        }
    }
}
