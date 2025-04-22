using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSharedLib.Models
{
    public class Dealer
    {
        public string Id { get; }

        public Dealer(string id)
        {
            Id = id;
        }

        public override string ToString() => $"Dealer({Id})";
    }
}
