using GameSharedLib.Messages;
using GameSharedLib.Models;

namespace GameSharedLib.Contracts;

public interface IGameSession
{
    Player Player { get; }
    Task StartAsync(Player player);
    Task HandleCommandAsync(GameCommand command);
}
