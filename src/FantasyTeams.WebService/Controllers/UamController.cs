using FantasyTeams.Commands;
using FantasyTeams.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<CommandResponse> Login([FromBody] UserLoginCommand userLoginCommand)
        {
            return await _mediator.Send(userLoginCommand);
        }
        [Authorize(Roles ="Admin")]
        [HttpPost("DeleteUser")]
        public async Task<CommandResponse> DeleteUser([FromBody] DeleteUserCommand deleteUserCommand)
        {
            return await _mediator.Send(deleteUserCommand);
        }
    }
}
