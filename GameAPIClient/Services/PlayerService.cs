using GameAPIClient.Services.Interface;
using GameSharedLib.Models;
using System.Collections.Concurrent;

public class PlayerService : IPlayerService
{
    private readonly ConcurrentDictionary<string, Player> _players = new();

    public Player CreatePlayer(string id)
    {
        var player = new Player(id);
        _players[id] = player;
        return player;
    }

    public Player? GetPlayer(string id)
    {
        _players.TryGetValue(id, out var player);
        return player;
    }

    public IEnumerable<Player> GetAllPlayers() => _players.Values;

    public bool UpdatePlayer(Player player)
    {
        if (_players.ContainsKey(player.Id))
        {
            _players[player.Id] = player;
            return true;
        }
        return false;
    }

    public IEnumerable<Player> GetTopPlayersByCoins(int top = 10)
    {
        var comparer = Comparer<Player>.Create((a, b) => a.Coins.CompareTo(b.Coins));
        var minHeap = new PriorityQueue<Player, int>();

        foreach (var player in _players.Values)
        {
            minHeap.Enqueue(player, player.Coins);

            if (minHeap.Count > top)
            {
                minHeap.Dequeue();
            }
        }

        var result = new List<Player>();
        while (minHeap.Count > 0)
        {
            result.Add(minHeap.Dequeue());
        }

        result.Reverse(); // Because min-heap
        return result;
    }
}
