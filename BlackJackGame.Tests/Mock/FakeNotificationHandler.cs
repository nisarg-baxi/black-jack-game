using GameSharedLib.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJackGame.Tests.Mock
{
    public class FakeNotificationHandler : INotificationHandler
    {
        public List<(string PlayerId, string Message)> Messages = new();

        public Task NotifyAsync(string playerId, string message)
        {
            Messages.Add((playerId, message));
            return Task.CompletedTask;
        }

        void INotificationHandler.Notify(string playerId, string message)
        {
            Messages.Add((playerId, message));
        }
    }
}
