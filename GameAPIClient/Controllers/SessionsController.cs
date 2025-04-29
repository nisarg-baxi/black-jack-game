using Microsoft.AspNetCore.Mvc;

namespace GameAPIClient.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using GameSharedLib.Models;
    using GameAPIClient.Services.Interface;
    using GameAPIClient.Model;

    [ApiController]
    [Route("api/[controller]")]
    public class SessionHistoryController : ControllerBase
    {
        private readonly IPlayerService _playerService;

        [HttpPost("record")]
        public IActionResult RecordSession([FromBody] SessionRecordRequest request)
        {
            var player = _playerService.GetPlayer(request.PlayerId);
            if (player == null)
                return NotFound("Player not found");

            player.SessionHistory.Add(new GameSessionResult
            {
                SessionId = request.SessionId,
                Won = request.Won,
                CoinsChange = request.CoinsChange,
                PlayedAt = DateTime.UtcNow
            });

            // Update coin balance
            if (request.CoinsChange > 0) player.AddCoins(request.CoinsChange);
            else if (request.CoinsChange < 0) player.SpendCoins(-request.CoinsChange);

            _playerService.UpdatePlayer(player);

            return Ok(new { Message = "Session recorded successfully" });
        }

        public SessionHistoryController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpGet("{playerId}")]
        public IActionResult GetSessionHistory(string playerId)
        {
            var player = _playerService.GetPlayer(playerId);
            if (player == null) return NotFound("Player not found");

            var sessions = player.SessionHistory
                .OrderByDescending(s => s.PlayedAt)
                .Select(s => new
                {
                    s.SessionId,
                    s.Won,
                    s.CoinsChange,
                    s.PlayedAt
                });

            return Ok(sessions);
        }
    }

}
