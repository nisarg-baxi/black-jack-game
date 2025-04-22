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
    public class WebSocketConnectionHandler
    {
        private readonly GameCommandRouter _router;

        public WebSocketConnectionHandler(GameCommandRouter router)
        {
            _router = router;
        }

        public async Task HandleConnectionAsync(WebSocket socket)
        {
            var buffer = new byte[4096];
            string playerId = "Unknown";

            try
            {
                while (socket.State == WebSocketState.Open)
                {
                    var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        Console.WriteLine($"❌ Client disconnected (graceful): {playerId}");
                        await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                        if (_router.DisconnectPlayer != null && playerId != "Unknown")
                        {
                            _router.DisconnectPlayer(playerId);
                        }
                        return;
                    }

                    var json = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    var command = SerializationHelper.FromJson<GameCommand>(json);

                    if (command != null)
                    {
                        playerId = command.PlayerId;
                        Console.WriteLine($"📥 Command from {playerId}: {command.Command}");
                        await _router.RouteAsync(command, socket);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Client disconnected (error): {playerId} — {ex.Message}");
            }
            finally
            {
                if (playerId != "Unknown")
                {
                    Console.WriteLine($"🧼 Cleaning up player: {playerId}");
                    _router.DisconnectPlayer(playerId);
                }
            }
        }

    }
}
