using GameSharedLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJackGameClient.Utils
{
    public static class AsciiCardHelper
    {
        public static string GetSuitSymbol(string suit) => suit switch
        {
            "Spades" => "♠",
            "Hearts" => "♥",
            "Diamonds" => "♦",
            "Clubs" => "♣",
            _ => "?"
        };

        public static List<string?> RenderHand(List<Card> hand)
        {
            var lines = new string[7];
            for (int i = 0; i < 7; i++)
                lines[i] = "";

            foreach (var card in hand)
            {
                var val = card.Value.PadRight(2).Substring(0, 2);
                var suit = GetSuitSymbol(card.Suit);

                lines[0] += "┌─────────┐ ";
                lines[1] += $"│ {val}      │ ";
                lines[2] += $"│         │ ";
                lines[3] += $"│    {suit}    │ ";
                lines[4] += $"│         │ ";
                lines[5] += $"│      {val} │ ";
                lines[6] += "└─────────┘ ";
            }

            return [.. lines];
        }
    }
}
