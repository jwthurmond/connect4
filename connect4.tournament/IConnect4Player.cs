using connect4.library;


namespace connect4.tournament
{
    public interface IConnect4Player
    {
        bool ShowBoardBeforeMove { get; }
        string Name { get; }
        int GetMove(GameBoard board);
    }

    public class RandomPlayer : IConnect4Player
    {
        public bool ShowBoardBeforeMove => false;
        public string Name => "Random";
        public int GetMove(GameBoard board)
        {
            var random = new Random();
            var column = random.Next(1, board.ColumnCountMax);
            return column;
        }
    }

    public class IncrementBy1:IConnect4Player
    {
        public bool ShowBoardBeforeMove => false;
        public string Name => "Increment by 1";
        private int _lastMove = 0;
        public int GetMove(GameBoard board)
        {
            _lastMove++ ;
            if(_lastMove> board.ColumnCountMax)
            {
                _lastMove = 1;
            }
            return _lastMove;

        }

    }

    public class Always4:IConnect4Player
    {
        public bool ShowBoardBeforeMove => false;
        public string Name => "Always 4";
        public int GetMove(GameBoard board)
        {
            return 4;
        }
    }

    public class Lowest : IConnect4Player
    {
        public bool ShowBoardBeforeMove => true;
        public string Name => "Lowest";
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
                    var rowCoord = board.RowCountMax - (row+1);
                    var currentCellValue = board.CurrentBoard[rowCoord, colCoord];
                    if (currentHeight >= minColHeight)
                    {
                        break;
                    }
                    if (currentCellValue != 0)
                    {
                        currentHeight = row+1;
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

    public class HumanInput:IConnect4Player
    {
        public bool ShowBoardBeforeMove => true;
        public string Name => "Human Input";
        public int GetMove(GameBoard board)
        {             
            Console.WriteLine("Enter a column number");
            var input = Console.ReadLine();
            var column = int.Parse(input);
            return column;
        }
    }
}
