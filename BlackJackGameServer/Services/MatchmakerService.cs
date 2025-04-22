using BlackJackGameServer.Sessions;
using GameSharedLib.Contracts;
using GameSharedLib.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJackGameServer.Services
{
    public class MatchmakerService
    {
        private readonly GameSessionManager _sessionManager;
        private readonly INotificationHandler _notifier;
        private readonly GameSessionRegistry _sessionRegistry;
        public MatchmakerService(GameSessionManager sessionManager, INotificationHandler notifier, GameSessionRegistry sessionRegistry)
        {
            _sessionManager = sessionManager;
            _notifier = notifier;
            _sessionRegistry = sessionRegistry;
        }

        public void AddPlayer(Player player)
        {
            _notifier.NotifyAsync(player.Id, "You've been matched with a bot dealer!");
            var session = new BlackjackGameSession(player, new Dealer("BotDealer"), _notifier);
            _sessionRegistry.Register(player, session); // register session here
            _sessionManager.StartSession(session, session.Player);
        }
    }

}
