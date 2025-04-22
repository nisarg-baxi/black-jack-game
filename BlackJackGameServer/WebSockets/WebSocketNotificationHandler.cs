using GameSharedLib.Contracts;
using GameSharedLib.Messages;
using GameSharedLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace BlackJackGameServer.WebSockets
{
    public class WebSocketNotificationHandler : INotificationHandler
    {
        private readonly Dictionary<string, WebSocket> _connections;

        public WebSocketNotificationHandler(Dictionary<string, WebSocket> connections)
        {
            _connections = connections;
        }

        public void Notify(string playerId, string message)
        {
            if (_connections.TryGetValue(playerId, out var socket))
            {
                var gameEvent = new GameEvent
                {
                    PlayerId = playerId,
                    EventType = "Notification",
                    Payload = message
                };

                var json = SerializationHelper.ToJson(gameEvent);
                var bytes = Encoding.UTF8.GetBytes(json);
                socket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        public async Task NotifyAsync(string playerId, string message)
        {
            if (_connections.TryGetValue(playerId, out var socket) && socket.State == WebSocketState.Open)
            {
                var gameEvent = new GameEvent
                {
                    PlayerId = playerId,
                    EventType = "Notification",
                    Payload = message
                };

                var json = SerializationHelper.ToJson(gameEvent);
                var bytes = Encoding.UTF8.GetBytes(json);
                await socket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}
