namespace GameSharedLib.Contracts;

public interface INotificationHandler
{
    void Notify(string playerId, string message);
}
