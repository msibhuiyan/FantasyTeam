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
    public class TeamController : ControllerBase
    {
        private readonly ILogger<TeamController> _logger;
        private readonly ITeamService _teamService;
        public TeamController(ILogger<TeamController> logger,
            ITeamService teamService)
        {
            _logger = logger;
            _teamService = teamService;
        }
        [HttpPost("CreateTeam")]
        public async Task<Team> CreateTeam([FromBody] CreateNewTeamCommand createNewTeamCommand)
        {
            await _teamService.CreateNewTeam(createNewTeamCommand);
            return null;
        }

        [HttpGet("GetTeam")]
        public async Task<Team> GetTeam([FromQuery] string TeamId)
        {
            return await _teamService.GetTeamInfo(TeamId);
        }
        [HttpGet("GetAllTeam")]
        public async Task<List<Team>> GetAllTeam()
        {
            return await _teamService.GetAllTeams();
        }
    }
}
