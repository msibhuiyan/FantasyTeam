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
    public class TeamController : ControllerBase
    {
        private readonly ILogger<TeamController> _logger;
        private readonly ITeamRepository _teamRepository;
        public TeamController(ILogger<TeamController> logger,
            ITeamRepository teamRepository)
        {
            _logger = logger;
            _teamRepository = teamRepository;
        }
        [HttpPost("CreateTeam")]
        public async Task<Team> CreateTeam([FromBody] CreateNewTeamCommand createNewTeamCommand)
        {
            return await _teamRepository.CreateAsync(createNewTeamCommand);
        }
    }
}
