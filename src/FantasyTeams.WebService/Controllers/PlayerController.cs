using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FantasyTeams.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly ILogger<PlayerController> _logger;
        public PlayerController(ILogger<PlayerController> logger)
        {
            _logger = logger;
        }
    }
}
