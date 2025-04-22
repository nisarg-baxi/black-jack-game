namespace GameSharedLib.Models;

public record Card(string Suit, string Value)
{
    public override string ToString() => $"{Value} of {Suit}";
}
