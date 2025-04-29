using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSharedLib.Models
{

    public class Player
    {
        public string Id { get; }
        public int Coins { get; private set; }
        public List<string> PurchasedItems { get; } = new();
        public List<GameSessionResult> SessionHistory { get; } = new();

        public Player(string id)
        {
            Id = id;
            Coins = 1000; // Default starting money
        }

        public void AddCoins(int amount) => Coins += amount;
        public bool SpendCoins(int amount)
        {
            if (Coins >= amount)
            {
                Coins -= amount;
                return true;
            }
            return false;
        }
    }
}
