namespace GameSharedLib.Messages;

public class GameCommand
{
    public string PlayerId { get; set; }
    public string Command { get; set; } // "Hit", "Stand", "Join", etc.
    public object? Data { get; set; }
}
