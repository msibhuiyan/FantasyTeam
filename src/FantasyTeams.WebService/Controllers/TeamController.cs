using FantasyTeams.Commands;
using FantasyTeams.Contracts;
using FantasyTeams.Entities;
using FantasyTeams.Repository;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Admin")]
        [HttpPost("CreateTeam")]
        public async Task<Team> CreateTeam([FromBody] CreateNewTeamCommand createNewTeamCommand)
        {
            await _teamService.CreateNewTeam(createNewTeamCommand);
            return null;
        }
        [Authorize(Roles = "Admin, Member")]
        [HttpGet("GetTeam")]
        public async Task<Team> GetTeam([FromQuery] string TeamId)
        {
            return await _teamService.GetTeamInfo(TeamId);
        }
        [Authorize(Roles = "Admin, Member")]
        [HttpPut("UpdateTeam")]
        public async Task UpdateTeam([FromBody] UpdateTeamCommand updateTeamCommand)
        {
            await _teamService.UpdateTeamInfo(updateTeamCommand);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllTeam")]
        public async Task<List<Team>> GetAllTeam()
        {
            return await _teamService.GetAllTeams();
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteTeam")]
        public async Task DeletePlayer([FromBody] DeleteTeamCommand deleteTeamCommand)
        {
            await _teamService.DeleteTeam(deleteTeamCommand);
        }
    }
}
