﻿using BlackJackGameServer.Utils;
using GameSharedLib.Contracts;
using GameSharedLib.Messages;
using GameSharedLib.Models;
using System.Net.Http.Json;
using System.Numerics;


namespace BlackJackGameServer.Sessions
{
    public class BlackjackGameSession : IGameSession
    {
        public Player Player { get; }
        private Dealer _dealer { get; }
        private INotificationHandler _notifier { get; }

        private readonly List<Card> _deck;
        private readonly List<Card> _playerHand = new();
        private readonly List<Card> _dealerHand = new();
        //private bool _playerStood = false;
        private bool _gameOver = false;

        public BlackjackGameSession(Player player, Dealer dealer, INotificationHandler notifier)
        {
            Player = player;
            _dealer = dealer;
            _notifier = notifier;
            _deck = Helper.BuildAndShuffleDeck();
        }

        public async Task StartAsync(Player player)
        {
            _notifier.Notify(player.Id, "🎲 Starting Blackjack session...");

            _playerHand.Add(Deal());
            _playerHand.Add(Deal());

            _dealerHand.Add(Deal());

            var playerScore = Helper.CalculateScore(_playerHand);
        
            await _notifier.NotifyAsync(player.Id, $"Your cards: {Helper.FormatHand(_playerHand)} [Score: {playerScore}]");
            await _notifier.NotifyAsync(player.Id, $"Dealer shows: {_dealerHand[0]}");

            await _notifier.NotifyAsync(player.Id, "Hit or Stand?");
            await Task.Delay(500); // simulate async work
        }


        public async Task HandleCommandAsync(GameCommand command)
        {
            if (_gameOver) return;

            switch (command.Command.ToLowerInvariant())
            {
                case "hit":
                    _playerHand.Add(Deal());
                    var score = Helper.CalculateScore(_playerHand);
                    await _notifier.NotifyAsync(Player.Id, $"🃏 You drew: {_playerHand.Last()} [Score: {score}]");

                    if (score > 21)
                    {
                        _gameOver = true;
                        await _notifier.NotifyAsync(Player.Id, "💥 Busted! You lose.");
                    }
                    else
                    {
                        await NotifyGameState("Continue: Hit or Stand?");
                    }
                    break;

                case "stand":
                    //_playerStood = true;
                    await PlayDealer();
                    await DetermineOutcome();
                    _gameOver = true;
                    break;
            }
        }

        private async Task PlayDealer()
        {
            int dealerScore = Helper.CalculateScore(_dealerHand);
            while (dealerScore < 17)
            {
                var card = Deal();
                _dealerHand.Add(card);
                dealerScore = Helper.CalculateScore(_dealerHand);
                await _notifier.NotifyAsync(Player.Id, $"💼 Dealer drew: {card} [Dealer Score: {dealerScore}]");
                await Task.Delay(500);
            }
        }

        private async Task DetermineOutcome()
        {
            int playerScore = Helper.CalculateScore(_playerHand);
            int dealerScore = Helper.CalculateScore(_dealerHand);

            string result;
            if (dealerScore > 21 || playerScore > dealerScore)
                result = "🎉 You win!";
            else if (playerScore == dealerScore)
                result = "🤝 Push! It's a tie.";
            else
                result = "🧊 Dealer wins.";

            await _notifier.NotifyAsync(Player.Id, result);

            // --- New Code to Record the Session Result ---
            int coinsChange = 0;
            bool won = false;

            // You can tune these values however you want:
            if (result.Contains("You win"))
            {
                coinsChange = 500; // Example reward
                won = true;
                Player.AddCoins(coinsChange);
            }
            else if (result.Contains("Dealer wins"))
            {
                coinsChange = -200; // Example loss
                won = false;
                Player.SpendCoins(200); // Lose 200 coins if lost
            }
            else
            {
                coinsChange = 0; // Tie
            }
            string sessionId = Guid.NewGuid().ToString();
            // Now record the game result
            Player.SessionHistory.Add(new GameSessionResult
            {
                SessionId = sessionId, // or reuse your matchmaking SessionId if available
                Won = won,
                CoinsChange = coinsChange,
                PlayedAt = DateTime.UtcNow
            });

            // Add this in class (or inject it if scaling up)
            HttpClient _httpClient = new();

            string apiBaseUrl = "http://localhost:5053"; // Your API server base address

            await _httpClient.PostAsJsonAsync($"{apiBaseUrl}/api/sessionhistory/record", new
            {
                PlayerId = Player.Id,
                SessionId = sessionId, // reuse your matchmaking sessionId if available
                Won = won,
                CoinsChange = coinsChange
            });

            //// Save updated player state
            //_playerService.UpdatePlayer(player);

        }

        private async Task NotifyGameState(string prompt)
        {
            int score = Helper.CalculateScore(_playerHand);
            var hand = string.Join(", ", _playerHand);
            await _notifier.NotifyAsync(Player.Id, $"Your cards: {hand} [Score: {score}]");
            await _notifier.NotifyAsync(Player.Id, prompt);
        }


        private Card Deal()
        {
            var card = _deck[0];
            _deck.RemoveAt(0);
            return card;
        }
    }
}
