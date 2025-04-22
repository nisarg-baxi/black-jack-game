namespace GameSharedLib.Contracts;

public interface INotificationHandler
{
    void Notify(string playerId, string message);

    Task NotifyAsync(string playerId, string message);
}
