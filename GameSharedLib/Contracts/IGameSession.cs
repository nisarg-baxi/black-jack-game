using GameSharedLib.Models;

namespace GameSharedLib.Contracts;

public interface IGameSession
{
    Task StartAsync(Player player);
}
