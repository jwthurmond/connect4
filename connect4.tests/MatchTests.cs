using connect4.library;
using connect4.tournament;
using Xunit;

namespace connect4.tests;

public class MatchTests
{
    [Fact]
    public void LosingPlayerStartsTheNextGame()
    {
        // PlayerA (P1) fills col 2 as sacrifices; PlayerB (P2) stacks col 1 for a
        // vertical 4-in-a-row win each game.  Because PlayerB wins game 1, the
        // "winner goes second" rule keeps PlayerA going first in game 2 as well.
        var playerA = new DeterministicPlayer("Player A", new[] { 2, 2, 2, 2, 2, 2, 2, 2 });
        var playerB = new DeterministicPlayer("Player B", new[] { 1, 1, 1, 1, 1, 1, 1, 1 });

        var match = new Match(
            roundsPerMatch: 2,
            playerAId: 1,
            playerA: playerA,
            playerBId: 2,
            playerB: playerB,
            initialPlayerAGoesFirst: true);

        match.RunMatch(showBoardAfterEachRound: false);

        Assert.Equal(new[] { true, true }, match.PlayerAStartedGame);
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
