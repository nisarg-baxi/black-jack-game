using System.ComponentModel.DataAnnotations;

namespace GameSharedLib.Messages;

public class GameEvent
{
    public required string PlayerId { get; set; }
    public required string EventType { get; set; } // e.g., "CardDealt", "GameOver"
    public object? Payload { get; set; }
}
