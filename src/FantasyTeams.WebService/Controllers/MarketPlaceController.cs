using FantasyTeams.Commands;
using FantasyTeams.Contracts;
using FantasyTeams.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FantasyTeams.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MarketPlaceController : ControllerBase
    {
        private readonly ILogger<MarketPlaceController> _logger;
        private readonly IMarketPlaceService _marketPlaceService;
        public MarketPlaceController(ILogger<MarketPlaceController> logger,
            IMarketPlaceService marketPlaceService)
        {
            _logger = logger;
            _marketPlaceService = marketPlaceService;
        }
        [HttpGet("GetAllPlayer")]
        public async Task<List<Player>> GetAllPlayer()
        {
            return await _marketPlaceService.GetAllMarketPlacePlayer();
        }

        [HttpGet("GetPlayer")]
        public async Task<Player> GetPlayer([FromQuery] string PlayerId)
        {
            return await _marketPlaceService.GetMarketPlacePlayer(PlayerId);
        }

        [HttpPost("PurchasePlayer")]
        public async Task PurchasePlayer([FromBody] PurchasePlayerCommand purchasePlayerCommand)
        {
            await _marketPlaceService.PurchasePlayer(purchasePlayerCommand);
        }
    }
}
