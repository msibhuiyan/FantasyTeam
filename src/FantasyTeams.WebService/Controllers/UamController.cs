using FantasyTeams.Commands.Uam;
using FantasyTeams.Models;
using FantasyTeams.Queries.Uam;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FantasyTeams.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UamController : ControllerBase
    {
        private readonly ILogger<UamController> _logger;
        private readonly IMediator _mediator;
        public UamController(ILogger<UamController> logger,
            
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<CommandResponse> Register([FromBody] UserRegistrationCommand userRegistrationCommand)
        {
            return await _mediator.Send(userRegistrationCommand);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("OnboardUser")]
        public async Task<CommandResponse> OnboardUser([FromBody] OnboardUserCommand onboardUserCommand)
        {
            return await _mediator.Send(onboardUserCommand);
        }
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<CommandResponse> Login([FromBody] UserLoginCommand userLoginCommand)
        {
            return await _mediator.Send(userLoginCommand);
        }
        [Authorize(Roles ="Admin")]
        [HttpDelete("DeleteUser")]
        public async Task<CommandResponse> DeleteUser([FromBody] DeleteUserCommand deleteUserCommand)
        {
            return await _mediator.Send(deleteUserCommand);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetUnAssignedUser")]
        public async Task<QueryResponse> GetUnAssignedUser([FromBody] GetUnAssignedUserQuery getUnAssignedUserQuery)
        {
            return await _mediator.Send(getUnAssignedUserQuery);
        }
    }
}
