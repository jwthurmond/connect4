using connect4.library;

namespace connect4.tournament;

public interface IConnect4Player
{
    bool ShowBoardBeforeMove { get; }
    public string Name { get; set; }
    ConsoleColor Color { get; set; }
    ConsoleColor AlternateColor { get; set; }
    int GetMove(GameBoard board);
    void StartNewGame();
    bool AcceptsCustomName { get; }
}

public class RandomPlayer : IConnect4Player
{
    public bool ShowBoardBeforeMove => false;
    public string Name
    {
        get { return "Random"; }
        set { }
    }

    public ConsoleColor Color { get; set; } = ConsoleColor.Red;
    public ConsoleColor AlternateColor { get; set; } = ConsoleColor.Green;
    public int GetMove(GameBoard board)
    {
        var random = new Random();
        var column = random.Next(1, board.ColumnCountMax);
        return column;
    }
    public void StartNewGame() { }
    public bool AcceptsCustomName => false;
}

public class IncrementBy1 : IConnect4Player
{
    public void StartNewGame() { _lastMove = 0; }
    public bool ShowBoardBeforeMove => false;
    public string Name
    {
        get { return "Increment by 1"; }
        set { }
    }
    public ConsoleColor Color { get; set; } = ConsoleColor.Yellow;
    public ConsoleColor AlternateColor { get; set; } = ConsoleColor.Blue;
    private int _lastMove = 0;
    public int GetMove(GameBoard board)
    {
        _lastMove++;
        if (_lastMove > board.ColumnCountMax)
        {
            _lastMove = 1;
        }
        return _lastMove;

    }
    public bool AcceptsCustomName => false;

}

public class Always4 : IConnect4Player
{
    public void StartNewGame() { }
    public bool ShowBoardBeforeMove => false;
    public string Name
    {
        get { return "Always 4"; }
        set { }
    }
    public ConsoleColor Color { get; set; } = ConsoleColor.Magenta;
    public ConsoleColor AlternateColor { get; set; } = ConsoleColor.Green;
    public int GetMove(GameBoard board)
    {
        return 4;
    }
    public bool AcceptsCustomName => false;
}

public class Lowest : IConnect4Player
{
    public void StartNewGame() { }
    public bool ShowBoardBeforeMove => false;
    public string Name
    {
        get { return "Lowest"; }
        set { }
    }
    public ConsoleColor Color { get; set; } = ConsoleColor.Blue;
    public ConsoleColor AlternateColor { get; set; } = ConsoleColor.Green;
    public int GetMove(GameBoard board)
    {
        var minCol = 0;
        var minColHeight = 100;
        for (var col = 1; col <= board.ColumnCountMax; col++)
        {
            var colCoord = col - 1;
            var currentHeight = 0;
            for (var row = 0; row < board.RowCountMax; row++)
            {
                var rowCoord = board.RowCountMax - (row + 1);
                var currentCellValue = board.CurrentBoard[rowCoord, colCoord];
                if (currentHeight >= minColHeight)
                {
                    break;
                }
                if (currentCellValue != 0)
                {
                    currentHeight = row + 1;
                    continue;
                }
                if (currentHeight < minColHeight && board.IsMoveValid(col))
                {
                    minCol = col;
                    minColHeight = currentHeight;
                    if (minColHeight == 0)
                    {
                        return minCol;
                    }
                }
                break;
            }
        }
        return minCol;
    }
    public bool AcceptsCustomName => false;
}

public class Highest : IConnect4Player
{
    public void StartNewGame() { }
    public bool ShowBoardBeforeMove => false;
    public string Name
    {
        get { return "Highest"; }
        set { }
    }
    public ConsoleColor Color { get; set; } = ConsoleColor.DarkGreen;
    public ConsoleColor AlternateColor { get; set; } = ConsoleColor.Red;
    public int GetMove(GameBoard board)
    {
        var maxCol = 0;
        var maxColHeight = -1;
        for (var col = 1; col <= board.ColumnCountMax; col++)
        {
            var colCoord = col - 1;
            var currentHeight = 0;
            for (var row = 0; row < board.RowCountMax; row++)
            {
                var rowCoord = board.RowCountMax - (row + 1);
                var currentCellValue = board.CurrentBoard[rowCoord, colCoord];
                if (currentCellValue != 0)
                {
                    currentHeight = row + 1;
                    continue;
                }
                if (currentHeight > maxColHeight && board.IsMoveValid(col))
                {
                    maxCol = col;
                    maxColHeight = currentHeight;
                    if (maxColHeight == board.RowCountMax - 2)
                    {
                        return maxCol;
                    }
                }
                break;
            }
        }
        return maxCol;
    }
    public bool AcceptsCustomName => false;
}

public class HumanInput : IConnect4Player
{
    public void StartNewGame() { }
    public bool ShowBoardBeforeMove => true;
    public string Name { get; set; } = "Human";
    public ConsoleColor Color { get; set; } = ConsoleColor.Green;
    public ConsoleColor AlternateColor { get; set; } = ConsoleColor.Red;
    public int GetMove(GameBoard board)
    {
        Console.ForegroundColor = Color;
        Console.Write($"Player {board.GetPlayer()}");
        Console.ResetColor();
        Console.Write(" Enter the Column:");
        var input = Console.ReadLine();
        var column = int.Parse(input);
        return column;
    }
    public bool AcceptsCustomName => true;
}
