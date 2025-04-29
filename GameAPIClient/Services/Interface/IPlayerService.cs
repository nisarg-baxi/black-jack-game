namespace GameAPIClient.Services.Interface
{
    using GameSharedLib.Models;
    using System.Collections.Generic;

    public interface IPlayerService
    {
        Player CreatePlayer(string id);
        Player? GetPlayer(string id);
        IEnumerable<Player> GetAllPlayers();
        bool UpdatePlayer(Player player);
        IEnumerable<Player> GetTopPlayersByCoins(int top);
    }

}
