using System.Net.WebSockets;
using System.Runtime.Loader;
using System.Threading;
using BlackJackGameServer.Services;
using BlackJackGameServer.WebSockets;
using GameSharedLib.Models;

var cts = new CancellationTokenSource();

// Trigger shutdown on Ctrl+C
Console.CancelKeyPress += (s, e) =>
{
    Console.WriteLine("🛑 Shutdown signal received...");
    cts.Cancel();
    e.Cancel = true;
};

AssemblyLoadContext.Default.Unloading += ctx =>
{
    Console.WriteLine("⏏️ Application unloading...");
    cts.Cancel();
};

var connections = new Dictionary<string, WebSocket>();
var notifier = new WebSocketNotificationHandler(connections);
var sessionManager = new GameSessionManager();
var sessionRegistry = new GameSessionRegistry();
var matchmaker = new MatchmakerService(sessionManager, notifier, sessionRegistry);
var router = new GameCommandRouter(matchmaker, connections, sessionRegistry);
var handler = new WebSocketConnectionHandler(router);
var server = new WebSocketGameServer(handler);

// ✅ Start WebSocket server in background
var port = Environment.GetEnvironmentVariable("GAME_SERVER_PORT") ?? "5000";
var serverUrl = $"ws://localhost:{port}/ws/";
_ = Task.Run(() => server.StartAsync(serverUrl, cts.Token));


Console.WriteLine("🎰 Game server running. Type player name or 'exit'");

while (!cts.Token.IsCancellationRequested)
{
    //Console.WriteLine("Enter player name: ");
    var input = Console.ReadLine();

    if (input?.ToLower() == "exit")
    {
        cts.Cancel();
        break;
    }

    //if (!string.IsNullOrWhiteSpace(input))
    //    matchmaker.AddPlayer(new Player(input));

    Console.WriteLine($"🧠 Active Players: {connections.Keys.Count}");
}

Console.WriteLine("Waiting for all sessions to complete...");
await sessionManager.WaitForAllSessionsAsync();
Console.WriteLine("✅ All games completed. Exiting cleanly.");
