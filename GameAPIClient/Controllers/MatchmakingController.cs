using GameAPIClient.Services;
using GameAPIClient.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GameAPIClient.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchmakingController : ControllerBase
    {
        private readonly PlayerService _playerService;
        private readonly IMatchmakingService _matchmakingService;

        public MatchmakingController(PlayerService playerService, IMatchmakingService matchmakingService)
        {
            _playerService = playerService;
            _matchmakingService = matchmakingService;
        }

        [HttpPost("join")]
        public IActionResult JoinMatchmaking([FromQuery] string playerId)
        {
            var player = _playerService.GetPlayer(playerId);
            if (player == null)
                return NotFound($"Player {playerId} not found");

            var sessionId = _matchmakingService.JoinMatchmaking(player);

            return Ok(new
            {
                PlayerId = player.Id,
                SessionId = sessionId,
                Message = "Successfully joined matchmaking!"
            });
        }
    }

}
