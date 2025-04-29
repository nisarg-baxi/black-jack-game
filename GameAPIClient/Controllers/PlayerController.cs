using GameAPIClient.Services.Interface;
using GameSharedLib.Models;
using Microsoft.AspNetCore.Mvc;

namespace GameAPIClient.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayersController : ControllerBase
    {
        private readonly IPlayerService _playerService;

        public PlayersController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpPost]
        public IActionResult CreatePlayer([FromBody] Player player)
        {
            var created = _playerService.CreatePlayer(player.Id);
            return CreatedAtAction(nameof(GetPlayer), new { id = created.Id }, created);
        }

        [HttpGet("{id}")]
        public IActionResult GetPlayer(string id)
        {
            var player = _playerService.GetPlayer(id);
            if (player == null)
                return NotFound();
            return Ok(player);
        }

        [HttpGet()]
        public IActionResult GetAllPlayer()
        {
            var players = _playerService.GetAllPlayers();
            if (players == null)
                return NotFound();
            return Ok(players);
        }
    }
}
