using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSharedLib.Models
{
    public class GameSessionResult
    {
        public required string SessionId { get; set; }
        public bool Won { get; set; }
        public int CoinsChange { get; set; }
        public DateTime PlayedAt { get; set; }
    }
}
