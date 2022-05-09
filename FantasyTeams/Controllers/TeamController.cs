using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FantasyTeams.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TeamController : ControllerBase
    {
        private readonly ILogger<TeamController> _logger;
        public TeamController(ILogger<TeamController> logger)
        {
            _logger = logger;
        }
    }
}
