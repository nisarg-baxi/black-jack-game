using BlackJackGameServer.Services;
using GameSharedLib.Messages;
using GameSharedLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace BlackJackGameServer.WebSockets
{
    public class GameCommandRouter
    {
        private readonly MatchmakerService _matchmaker;
        private readonly Dictionary<string, WebSocket> _playerConnections;
        private readonly GameSessionRegistry _sessionRegistry;

        public GameCommandRouter(MatchmakerService matchmaker, Dictionary<string, WebSocket> playerConnections, GameSessionRegistry registry)
        {
            _matchmaker = matchmaker;
            _playerConnections = playerConnections;
            _sessionRegistry = registry;
        }

        public async Task RouteAsync(GameCommand command, WebSocket socket)
        {
            if (!_playerConnections.ContainsKey(command.PlayerId)) {
                _playerConnections[command.PlayerId] = socket;
                Console.WriteLine($"🧠 Active Players: {_playerConnections.Keys.Count}");
            }
            

            switch (command.Command.ToLowerInvariant())
            {
                case "join":
                    var player = new Player(command.PlayerId);
                    _matchmaker.AddPlayer(player);
                    Console.WriteLine($"Player {player.Id} joined");
                    break;

                case "hit":
                case "stand":
                    var session = _sessionRegistry.GetSessionFor(command.PlayerId);
                    if (session != null)
                    {
                        await session.HandleCommandAsync(command); // your session handles "hit" or "stand"
                    }
                    break;

                default:
                    Console.WriteLine($"⚠️ Unknown command: {command.Command}");
                    break;
            }
        }

        // ✅ Called from connection handler when client disconnects
        public void DisconnectPlayer(string playerId)
        {
            Console.WriteLine($"🧹 Cleaning up connection for: {playerId}");

            if (_playerConnections.ContainsKey(playerId))
                _playerConnections.Remove(playerId);

            if (_sessionRegistry.GetSessionFor(playerId) != null)
            {
                _sessionRegistry.Remove(playerId);
                Console.WriteLine($"🧹 Removed session for: {playerId}");
            }
        }
    }
}
