using FantasyTeams.Commands;
using FantasyTeams.Contracts;
using FantasyTeams.Entities;
using FantasyTeams.Models;
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
    public class TeamController : ControllerBase
    {
        private readonly ILogger<TeamController> _logger;
        private readonly IMediator _mediator;
        public TeamController(ILogger<TeamController> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("CreateTeam")]
        public async Task<Team> CreateTeam([FromBody] CreateNewTeamCommand createNewTeamCommand)
        {
            return null;
            //return await _mediator.Send(createNewTeamCommand);
        }
        [Authorize(Roles = "Admin, Member")]
        [HttpGet("GetTeam")]
        public async Task<Team> GetTeam([FromQuery] string TeamId)
        {
            return null;
            //return await _teamService.GetTeamInfo(TeamId);
        }
        [Authorize(Roles = "Admin, Member")]
        [HttpPut("UpdateTeam")]
        public async Task<CommandResponse> UpdateTeam([FromBody] UpdateTeamCommand updateTeamCommand)
        {
            return await _mediator.Send(updateTeamCommand);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllTeam")]
        public async Task<List<Team>> GetAllTeam()
        {
            return null;
            //return await _teamService.GetAllTeams();
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteTeam")]
        public async Task<CommandResponse> DeletePlayer([FromBody] DeleteTeamCommand deleteTeamCommand)
        {
            return await _mediator.Send(deleteTeamCommand);
        }
    }
}
