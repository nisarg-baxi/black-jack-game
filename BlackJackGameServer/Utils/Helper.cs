using GameSharedLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJackGameServer.Utils
{
    internal class Helper
    {
        // --- Helpers ---

        internal static List<Card> BuildAndShuffleDeck()
        {
            string[] suits = { "Hearts", "Diamonds", "Clubs", "Spades" };
            string[] values = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };

            var deck = (from suit in suits
                        from value in values
                        select new Card(suit, value)).ToList();

            var rng = new Random();
            return deck.OrderBy(_ => rng.Next()).ToList();
        }

        internal static int CalculateScore(List<Card> hand)
        {
            int score = 0, aces = 0;
            foreach (var card in hand)
            {
                if (int.TryParse(card.Value, out var value)) score += value;
                else if (card.Value == "A") { aces++; score += 11; }
                else score += 10;
            }

            while (score > 21 && aces-- > 0) score -= 10;

            return score;
        }

        internal static string FormatHand(List<Card> hand)
        {
            var sb = new StringBuilder();
            foreach (var card in hand)
                sb.Append($"{card}, ");
            return sb.ToString().TrimEnd(',', ' ');
        }
    }

   
}
