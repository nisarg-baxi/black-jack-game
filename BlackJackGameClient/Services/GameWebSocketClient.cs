using BlackJackGameClient.Utils;
using GameSharedLib.Messages;
using GameSharedLib.Models;
using GameSharedLib.Utils;
using System.Net.WebSockets;
using System.Text;

namespace BlackJackGameClient.Services
{
    public class GameWebSocketClient
    {
        private readonly Uri _serverUri;
        private readonly string _playerId;
        private readonly ClientWebSocket _socket = new();

        public GameWebSocketClient(string url, string playerId)
        {
            _serverUri = new Uri(url);
            _playerId = playerId;
        }

        public async Task ConnectAsync()
        {
            await _socket.ConnectAsync(_serverUri, CancellationToken.None);
            Console.WriteLine($"✅ Connected to server as {_playerId}");

            _ = Task.Run(ReceiveLoopAsync); // receive messages in background
        }

        public async Task RunInteractiveLoopAsync()
        {
            // Send join immediately
            await SendCommandAsync("Join");

            while (_socket.State == WebSocketState.Open)
            {
                Console.Write("➤ Enter command (Hit / Stand): ");
                var input = Console.ReadLine()?.ToLowerInvariant();

                if (input == "exit") break;
                if (input == "hit" || input == "stand")
                {
                    await SendCommandAsync(input);
                }
            }

            Console.WriteLine("👋 Disconnecting...");
            await _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Bye", CancellationToken.None);
        }

        private async Task SendCommandAsync(string command)
        {
            var cmd = new GameCommand
            {
                PlayerId = _playerId,
                Command = command
            };

            var json = SerializationHelper.ToJson(cmd);
            var bytes = Encoding.UTF8.GetBytes(json);
            await _socket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private async Task ReceiveLoopAsync()
        {
            var buffer = new byte[4096];

            while (_socket.State == WebSocketState.Open)
            {
                try
                {
                    var result = await _socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    if (result.MessageType == WebSocketMessageType.Close) break;

                    var json = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    var gameEvent = SerializationHelper.FromJson<GameEvent>(json);
                    string? _payLoad = gameEvent?.Payload?.ToString();
                    if (_payLoad != null && _payLoad.StartsWith("Your cards:"))
                    {
                        var handStr = _payLoad.Split("Your cards:")[1].Split("[")[0].Trim();
                        var cards = handStr.Split(',')
                            .Select(s => s.Trim())
                            .Select(ParseCardFromText)
                            .ToList();

                        var asciiLines = AsciiCardHelper.RenderHand(cards);
                        foreach (var line in asciiLines)
                            Console.WriteLine(line);
                    }
                    else
                    {
                        Console.WriteLine($"📩 {_payLoad}");
                    }

                    if (_payLoad != null && _payLoad.ToLower().Contains("hit or stand"))
                    {
                        Console.Write("➤ Enter command (Hit / Stand): ");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Receive failed: {ex.Message}");
                    break;
                }
            }
        }
        private static Card ParseCardFromText(string input)
        {
            var parts = input.Split(" of ");
            return new Card(parts[1], parts[0]); // (Suit, Value)
        }


    }


}
