using connect4.library;
using connect4.tournament;

namespace connect4.exampleplayer;

/// <summary>
/// Example custom player: always tries the center column first,
/// then expands outward. Drop this DLL into the plugins/ folder
/// next to connect4.runtournament to include it in tournaments.
/// </summary>
public class CenterPlayer : IConnect4Player
{
    public bool ShowBoardBeforeMove => false;
    public string Name { get; set; } = "Center";
    public ConsoleColor Color { get; set; } = ConsoleColor.Cyan;
    public ConsoleColor AlternateColor { get; set; } = ConsoleColor.DarkCyan;

    public void StartNewGame() { }

    public bool AcceptsCustomName => false;

    public int GetMove(GameBoard board)
    {
        var center = (board.ColumnCountMax + 1) / 2;
        for (var offset = 0; offset <= board.ColumnCountMax / 2; offset++)
        {
            var left = center - offset;
            var right = center + offset;
            if (left >= 1 && board.IsMoveValid(left)) return left;
            if (right <= board.ColumnCountMax && board.IsMoveValid(right)) return right;
        }
        return center;
    }
}
