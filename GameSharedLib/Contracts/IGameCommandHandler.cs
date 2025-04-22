using GameSharedLib.Messages;

namespace GameSharedLib.Contracts;

public interface IGameCommandHandler
{
    public Task HandleAsync(GameCommand command);
}
