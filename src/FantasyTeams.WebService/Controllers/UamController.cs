using FantasyTeams.Commands.Uam;
using FantasyTeams.Models;
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
            string role = User.FindFirst(ClaimTypes.Role).Value;
            if(role == "Admin")
            {
                return await _mediator.Send(new OnboardUserCommand
                {
                    Email = userRegistrationCommand.Email,
                    Password = userRegistrationCommand.Password,
                    FirstName = userRegistrationCommand.FirstName,
                    LastName = userRegistrationCommand.LastName,
                    Country = userRegistrationCommand.Country
                });
            }
            return await _mediator.Send(userRegistrationCommand);
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
        [HttpPost("GetUnAssignedUser")]
        public async Task<CommandResponse> GetUnAssignedUser([FromBody] DeleteUserCommand deleteUserCommand)
        {
            return await _mediator.Send(deleteUserCommand);
        }
    }
}
