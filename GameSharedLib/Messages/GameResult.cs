namespace GameSharedLib.Messages;

public class GameResult
{
    public string Result { get; set; } = "InProgress"; // "Win", "Lose", "Tie", etc.
    public int PlayerScore { get; set; }
    public int DealerScore { get; set; }
}
