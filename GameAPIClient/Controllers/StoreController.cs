namespace GameAPIClient.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using GameSharedLib.Models;
    using GameAPIClient.Services.Interface;
    using GameAPIClient.Services;

    [ApiController]
    [Route("api/[controller]")]
    public class StoreController : ControllerBase
    {
        private readonly IStoreService _storeService;
        private readonly IPlayerService _playerService;

        public StoreController(IStoreService storeService, IPlayerService playerService)
        {
            _storeService = storeService;
            _playerService = playerService;
        }

        [HttpGet("items")]
        public IActionResult GetStoreItems()
        {
            return Ok(_storeService.GetStoreItems());
        }

        [HttpPost("purchase")]
        public IActionResult PurchaseItem([FromQuery] string playerId, [FromQuery] string itemName)
        {
            var player = _playerService.GetPlayer(playerId);
            if (player == null) return NotFound("Player not found");

            var item = _storeService.GetItemByName(itemName);
            if (item == null) return NotFound("Item not found");

            if (!player.SpendCoins(item.Cost))
                return BadRequest("Not enough coins to purchase item");

            player.PurchasedItems.Add(item.Name);
            _playerService.UpdatePlayer(player);

            return Ok(new
            {
                Message = $"Successfully purchased {item.Name}!",
                RemainingCoins = player.Coins,
                Inventory = player.PurchasedItems
            });
        }
    }

}
