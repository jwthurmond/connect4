using connect4.library;
using connect4.tournament;
using Xunit;

namespace connect4.tests;

public class MatchTests
{
    [Fact]
    public void LosingPlayerStartsTheNextGame()
    {
        var playerA = new DeterministicPlayer("Player A", new[] { 1, 1, 2, 1, 2, 1, 2, 1 });
        var playerB = new DeterministicPlayer("Player B", new[] { 1, 1, 2, 1, 2, 1, 2, 1 });

        var match = new Match(
            roundsPerMatch: 2,
            playerAId: 1,
            playerA: playerA,
            playerBId: 2,
            playerB: playerB,
            initialPlayerAGoesFirst: true);

        match.RunMatch(showBoardAfterEachRound: false);

        Console.WriteLine($"Winner game1={match.Games[0].Winner}, winner game2={match.Games[1].Winner}, playerA starts={string.Join(",", playerA.FirstTurnGames)}");
        Assert.Equal(new[] { 1, 2 }, playerA.FirstTurnGames);
        Assert.Empty(playerB.FirstTurnGames);
    }

    private sealed class DeterministicPlayer : IConnect4Player
    {
        private readonly Queue<int> _moves;
        private int _currentGameNumber;

        public DeterministicPlayer(string name, IEnumerable<int> moves)
        {
            Name = name;
            _moves = new Queue<int>(moves);
        }

        public List<int> FirstTurnGames { get; } = new();

        public bool ShowBoardBeforeMove => false;
        public string Name { get; set; }
        public ConsoleColor Color { get; set; } = ConsoleColor.Red;
        public ConsoleColor AlternateColor { get; set; } = ConsoleColor.Green;
        public bool AcceptsCustomName => false;

        public int GetMove(GameBoard board)
        {
            if (board.MoveCount == 0)
            {
                FirstTurnGames.Add(_currentGameNumber);
            }

            return _moves.Count > 0 ? _moves.Dequeue() : 1;
        }

        public void StartNewGame()
        {
            _currentGameNumber++;
        }
    }
}
