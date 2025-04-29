namespace GameAPIClient.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using GameSharedLib.Models;
    using GameAPIClient.Services.Interface;

    [ApiController]
    [Route("api/[controller]")]
    public class LeaderboardController : ControllerBase
    {
        private readonly IPlayerService _playerService;

        public LeaderboardController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpGet]
        public IActionResult GetLeaderboard([FromQuery] int top = 10)
        {
            var players = _playerService.GetTopPlayersByCoins(top)
                .Select(p => new
                {
                    PlayerId = p.Id,
                    Coins = p.Coins,
                    Items = p.PurchasedItems.Count,
                    GamesPlayed = p.SessionHistory.Count
                });

            return Ok(players);
        }
    }

}
