namespace GameAPIClient.Services.Interface
{
    using GameSharedLib.Models;
    using System.Collections.Generic;

    public interface IMatchmakingService
    {
        string JoinMatchmaking(Player player);
        IEnumerable<string> GetPlayersInSession(string sessionId);
        string? GetSessionForPlayer(string playerId);
    }

}
