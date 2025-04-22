using GameSharedLib.Contracts;
using GameSharedLib.Models;

namespace BlackJackGameServer.Services;

public class GameSessionRegistry
{
    private readonly Dictionary<string, IGameSession> _sessions = new();

    public void Register(Player player, IGameSession session)
    {
        _sessions[player.Id] = session;
    }

    public IGameSession? GetSessionFor(string playerId)
    {
        _sessions.TryGetValue(playerId, out var session);
        return session;
    }
    public void Remove(string playerId)
    {
        _sessions.Remove(playerId);
    }
}
