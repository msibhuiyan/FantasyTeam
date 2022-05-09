using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FantasyTeams.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransferController : ControllerBase
    {
        private readonly ILogger<TransferController> _logger;
        public TransferController(ILogger<TransferController> logger)
        {
            _logger = logger;
        }
    }
}
