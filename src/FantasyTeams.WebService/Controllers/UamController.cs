using FantasyTeams.Commands;
using FantasyTeams.Contracts;
using FantasyTeams.Models;
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
        private readonly IUamService _uamService;
        public UamController(ILogger<UamController> logger,
            IUamService uamService)
        {
            _logger = logger;
            _uamService = uamService;
        }
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task Register([FromBody] UserRegistrationCommand userRegistrationCommand)
        {
            await _uamService.RegisterUser(userRegistrationCommand);
        }
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<string> Login([FromBody] UserLoginCommand userLoginCommand)
        {
            return await _uamService.UserLogin(userLoginCommand);
        }
    }
}
