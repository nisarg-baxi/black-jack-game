using BlackJackGameClient.Services;

Console.Write("Enter your player name: ");
var playerId = Console.ReadLine();
while (string.IsNullOrEmpty(playerId) && string.IsNullOrEmpty(playerId)) {
    Console.WriteLine("Player name can not be empty,please try again.. ");
    playerId = Console.ReadLine();
}

var client = new GameWebSocketClient("ws://localhost:5000/ws", playerId);
await client.ConnectAsync();
await client.RunInteractiveLoopAsync();