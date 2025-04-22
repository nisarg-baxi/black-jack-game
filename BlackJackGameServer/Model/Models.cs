using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJackGameServer.Model
{

    public record Player(string Id);

    public record Card(string Suit, string Value)
    {
        public override string ToString() => $"{Value} of {Suit}";
    }

    public class Deck
    {
        private static readonly string[] Suits = { "Hearts", "Diamonds", "Clubs", "Spades" };
        private static readonly string[] Values = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
        private readonly List<Card> _cards = new();
        private readonly Random _random = new();

        public Deck()
        {
            foreach (var suit in Suits)
                foreach (var value in Values)
                    _cards.Add(new Card(suit, value));
            Shuffle();
        }

        public void Shuffle()
        {
            for (int i = _cards.Count - 1; i > 0; i--)
            {
                int j = _random.Next(i + 1);
                (_cards[i], _cards[j]) = (_cards[j], _cards[i]);
            }
        }

        public Card? Deal() => _cards.Count > 0 ? _cards[0] : null;
    }
}
