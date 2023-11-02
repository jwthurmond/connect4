using connect4.library;
using System.Drawing;

namespace connect4.tournament
{
    public interface IConnect4Player
    {
        bool ShowBoardBeforeMove { get; }
        string Name { get; }
        ConsoleColor Color { get; set; }
        int GetMove(GameBoard board);
        void StartNewGame();
    }

    public class RandomPlayer : IConnect4Player
    {
        public bool ShowBoardBeforeMove => false;
        public string Name => "Random";
        public ConsoleColor Color { get; set; } = ConsoleColor.Red;
        public int GetMove(GameBoard board)
        {
            var random = new Random();
            var column = random.Next(1, board.ColumnCountMax);
            return column;
        }
        public void StartNewGame() { }
    }

    public class IncrementBy1 : IConnect4Player
    {
        public void StartNewGame() { _lastMove = 0; }
        public bool ShowBoardBeforeMove => false;
        public string Name => "Increment by 1";
        public ConsoleColor Color { get; set; } = ConsoleColor.Yellow;
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

    }

    public class Always4 : IConnect4Player
    {
        public void StartNewGame() { }
        public bool ShowBoardBeforeMove => false;
        public string Name => "Always 4";
        public ConsoleColor Color { get; set; } = ConsoleColor.Magenta;
        public int GetMove(GameBoard board)
        {
            return 4;
        }
    }

    public class Lowest : IConnect4Player
    {
        public void StartNewGame() { }
        public bool ShowBoardBeforeMove => false;
        public string Name => "Lowest";
        public ConsoleColor Color { get; set; } = ConsoleColor.Blue;
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
    }

    public class Highest : IConnect4Player
    {
        public void StartNewGame() { }
        public bool ShowBoardBeforeMove => false;
        public string Name => "Highest";
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
    }

    public class HumanInput : IConnect4Player
    {
        public void StartNewGame() { }
        public bool ShowBoardBeforeMove => true;
        public string Name => "Human Input";
        public ConsoleColor Color { get; set; } = ConsoleColor.Green;
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
    }
}
