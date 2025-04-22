using BlackJackGame.Tests.Mock;
using BlackJackGameServer.Services;
using BlackJackGameServer.Sessions;
using GameSharedLib.Contracts;
using GameSharedLib.Models;

namespace BlackJackGame.Tests
{
    public class BlackjackGameSessionTests
    {
        [Fact]
        public async Task Player_Hits_And_Busts_Should_Send_Bust_Message()
        {
            // Arrange
            var notifier = new FakeNotificationHandler();
            var dealer = new Dealer("House");
            var player = new Player("test1");
            var session = new BlackjackGameSession(player, dealer, notifier);

            // Setup a high initial score (simulate 20)
            var deckField = typeof(BlackjackGameSession).GetField("_deck", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var deck = new List<Card>
            {
                new Card("Hearts", "K"), // will be dealt first
                new Card("Clubs", "Q"),
                new Card("Diamonds", "5"), // causes bust
                new Card("Spade", "5"), // causes bust
                new Card("Clubs", "5"), // causes busts

            };
            deckField?.SetValue(session, deck);

            // Act
            await session.StartAsync(player);
            await session.HandleCommandAsync(new GameSharedLib.Messages.GameCommand { PlayerId = "test1", Command = "Hit" });
            await session.HandleCommandAsync(new GameSharedLib.Messages.GameCommand { PlayerId = "test1", Command = "Hit" });

            // Assert
            Assert.Contains(notifier.Messages, m => m.Message.Contains("busted", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void Can_Register_And_Remove_Session()
        {
            var registry = new GameSessionRegistry();
            var session = new TestSession();
            var player = new Player("unit_test");

            registry.Register(player, session);
            Assert.NotNull(registry.GetSessionFor("unit_test"));

            registry.Remove("unit_test");
            Assert.Null(registry.GetSessionFor("unit_test"));
        }

        private class TestSession : IGameSession
        {
            Player IGameSession.Player => throw new NotImplementedException();

            public Task HandleCommandAsync(GameSharedLib.Messages.GameCommand command) => Task.CompletedTask;
            public Task StartAsync(Player player) => Task.CompletedTask;
        }
    }
}
