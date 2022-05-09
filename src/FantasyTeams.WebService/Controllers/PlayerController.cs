using FantasyTeams.Commands;
using FantasyTeams.Entities;
using FantasyTeams.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FantasyTeams.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly ILogger<PlayerController> _logger;
        private readonly IPlayerRepository _playerRepository;
        public PlayerController(ILogger<PlayerController> logger,
            IPlayerRepository playerRepository)
        {
            _logger = logger;
            _playerRepository = playerRepository;
        }

        [HttpPost("CreatePlayer")]
        public async Task<Player> CreatePlayer([FromBody] CreateNewPlayerCommand createNewPlayerCommand)
        {
            return await _playerRepository.CreateAsync(createNewPlayerCommand);
        }
    }
}
