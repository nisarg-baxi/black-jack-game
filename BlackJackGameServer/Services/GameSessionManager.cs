using GameSharedLib.Contracts;
using GameSharedLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJackGameServer.Services
{
    public class GameSessionManager
    {
        private readonly List<Task> _runningSessions = new();

        public void StartSession(IGameSession session, Player player)
        {
            var task = Task.Run(() => session.StartAsync(player));
            _runningSessions.Add(task);
        }


        public async Task WaitForAllSessionsAsync()
        {
            await Task.WhenAll(_runningSessions);
        }
    }

}
