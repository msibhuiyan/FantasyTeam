using FantasyTeams.Commands;
using FantasyTeams.Commands.Team;
using FantasyTeams.Contracts;
using FantasyTeams.Entities;
using FantasyTeams.Models;
using FantasyTeams.Queries;
using FantasyTeams.Repository;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Security.Claims;
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
        public async Task<CommandResponse> CreateTeam([FromBody] CreateTeamCommand createNewTeamCommand)
        {
            return await _mediator.Send(createNewTeamCommand);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetTeam")]
        public async Task<QueryResponse> GetTeam([FromQuery] string teamId, string teamName)
        {
            return await _mediator.Send(new GetTeamQuery { 
                TeamId = teamId,
                TeamName = teamName
            });;
        }
        [Authorize(Roles = "Member")]
        [HttpGet("GetMyTeam")]
        public async Task<QueryResponse> GetMyTeam()
        {
            var teamId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return await _mediator.Send(new GetTeamQuery
            {
                TeamId = teamId
            });
        }
        [Authorize(Roles = "Admin, Member")]
        [HttpPut("UpdateTeam")]
        public async Task<CommandResponse> UpdateTeam([FromBody] UpdateTeamCommand updateTeamCommand)
        {
            var teamId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            string role = User.FindFirst(ClaimTypes.Role).Value;
            if(role == "Admin")
            {
                return await _mediator.Send(updateTeamCommand);
            }
            if(teamId == updateTeamCommand.TeamId)
            {
                return await _mediator.Send(updateTeamCommand);
            }
            return CommandResponse.FailureForBidden(new string[] { "Can not update other team info" });
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllTeam")]
        public async Task<QueryResponse> GetAllTeam()
        {
            return await _mediator.Send(new GetAllTeamQuery()); ;
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteTeam")]
        public async Task<CommandResponse> DeletePlayer([FromBody] DeleteTeamCommand deleteTeamCommand)
        {
            return await _mediator.Send(deleteTeamCommand);
        }
    }
}
