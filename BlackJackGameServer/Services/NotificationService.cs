using GameSharedLib.Contracts;

namespace BlackJackGameServer.Services
{
    public class ConsoleNotificationHandler : INotificationHandler
    {
        public void Notify(string playerId, string message)
        {
            Console.WriteLine($"[NOTIFY {playerId}]: {message}");
        }

        public Task NotifyAsync(string playerId, string message)
        {
            Console.WriteLine($"[NOTIFY {playerId}]: {message}");
            return Task.CompletedTask;
        }
    }

}
