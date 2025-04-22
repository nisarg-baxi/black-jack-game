using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJackGameServer.Utils
{
    public static class ServerLogger
    {
        private static readonly string logFile = "server.log";

        public static void Log(string message)
        {
            File.AppendAllText(logFile, $"[{DateTime.Now}] {message}{Environment.NewLine}");
        }
    }

}
