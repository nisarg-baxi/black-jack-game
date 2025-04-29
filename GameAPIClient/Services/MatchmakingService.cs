using GameAPIClient.Services.Interface;
using GameSharedLib.Models;
using System.Collections.Concurrent;

namespace GameAPIClient.Services
{
    public class MatchmakingService: IMatchmakingService
    {
        private readonly ConcurrentDictionary<string, string> _playerSessions = new(); // PlayerId -> SessionId
        private readonly ConcurrentDictionary<string, List<string>> _sessionPlayers = new(); // SessionId -> List of PlayerIds

        private readonly int _maxPlayersPerSession = 1; // For now, single player per blackjack game

        public string JoinMatchmaking(Player player)
        {
            if (_playerSessions.ContainsKey(player.Id))
            {
                // Already matched
                return _playerSessions[player.Id];
            }

            // Try to find an open session
            foreach (var session in _sessionPlayers)
            {
                if (session.Value.Count < _maxPlayersPerSession)
                {
                    session.Value.Add(player.Id);
                    _playerSessions[player.Id] = session.Key;
                    return session.Key;
                }
            }

            // No open session, create new session
            var newSessionId = Guid.NewGuid().ToString();
            _sessionPlayers[newSessionId] = new List<string> { player.Id };
            _playerSessions[player.Id] = newSessionId;
            return newSessionId;
        }

        public IEnumerable<string> GetPlayersInSession(string sessionId)
        {
            if (_sessionPlayers.TryGetValue(sessionId, out var players))
            {
                return players;
            }
            return Enumerable.Empty<string>();
        }

        public string? GetSessionForPlayer(string playerId)
        {
            if (_playerSessions.TryGetValue(playerId, out var sessionId))
            {
                return sessionId;
            }
            return null;
        }
    }
}
