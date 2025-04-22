namespace GameSharedLib.Models;

public class GameState
{
    public List<Card> PlayerCards { get; set; } = new();
    public List<Card> DealerCards { get; set; } = new();
    public int PlayerScore { get; set; }
    public int DealerScore { get; set; }
    public string Status { get; set; } = "InProgress"; // "Won", "Lost", "Busted", "Push"
}
