using FantasyTeams.Commands;
using FantasyTeams.Queries;
using FantasyTeams.Contracts;
using FantasyTeams.Entities;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Admin, Member")]
        [HttpGet("GetAllPlayer")]
        public async Task<List<Player>> GetAllPlayer()
        {
            return await _marketPlaceService.GetAllMarketPlacePlayer();
        }
        [Authorize(Roles = "Admin, Member")]
        [HttpGet("GetPlayer")]
        public async Task<Player> GetPlayer([FromQuery] string PlayerId)
        {
            return await _marketPlaceService.GetMarketPlacePlayer(PlayerId);
        }
        [Authorize(Roles = "Admin, Member")]
        [HttpPost("FindPlayer")]
        public async Task<List<Player>> FindPlayer(
            [FromBody] FindPlayerQuery findPlayerQuery)
        {
            return await _marketPlaceService.FindMarketPlacePlayer(findPlayerQuery);
        }
        [Authorize(Roles = "Member")]
        [HttpPost("PurchasePlayer")]
        public async Task PurchasePlayer([FromBody] PurchasePlayerCommand purchasePlayerCommand)
        {
            await _marketPlaceService.PurchasePlayer(purchasePlayerCommand);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("DeletePlayer")]
        public async Task DeletePlayer([FromBody] DeletePlayerCommand deletePlayerCommand)
        {
            await _marketPlaceService.DeletePlayer(deletePlayerCommand);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("CreatePlayer")]
        public async Task CreateNewMarketPlacePlayer([FromBody] 
        CreateNewMarketPlacePlayerCommand createNewMarketPlacePlayerCommand)
        {
            await _marketPlaceService.CreateNewMarketPlacePlayer(createNewMarketPlacePlayerCommand);
        }
    }
}
