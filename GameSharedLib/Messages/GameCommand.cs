namespace GameSharedLib.Messages;

public class GameCommand
{
    public required string PlayerId { get; set; }
    public required string Command { get; set; } // "Hit", "Stand", "Join", etc.
    public object? Data { get; set; }
}
