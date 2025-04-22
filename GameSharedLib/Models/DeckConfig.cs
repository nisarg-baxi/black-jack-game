namespace GameSharedLib.Models;

public class DeckConfig
{
    public string[] Suits { get; set; } = { "Hearts", "Diamonds", "Clubs", "Spades" };
    public string[] Values { get; set; } = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
}
