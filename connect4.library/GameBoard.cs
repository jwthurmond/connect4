using System.Reflection;

namespace connect4.library;
public class GameBoard
{
    public class Corrdinate
    {
        public int Row { get; set; }
        public int Column { get; set; }
    }

    public GameBoard(int width = 7, int height = 6)
    {
        ColumnCountMax = width;
        RowCountMax = height;
        boardState = new int[RowCountMax, ColumnCountMax];
        MaxMoves = RowCountMax * ColumnCountMax;
        Initialize(this);
    }

    public void Initialize(GameBoard board)
    {
        for (int i = 0; i < RowCountMax; i++)
        {
            for (int j = 0; j < ColumnCountMax; j++)
            {
                board.boardState[i, j] = 0;
            }
        }
    }

    public List<Corrdinate>? WinningSet { get; private set; }
    public Corrdinate? LastMove { get; private set; }
    private int[,] boardState { get; init; }

    public int[,] CurrentBoard { get { return boardState; } }
    public int RowCountMax { get; init; }
    public int ColumnCountMax { get; init; }
    public const int InvalidMoveCountMax = 5;
    public int MoveCount { get; private set; } = 0;
    public int InvalidMoveCount { get; private set; } = 0;
    public int MaxMoves { get; init; }
    public int Player { get; private set; } = 1;
    public int Winner { get; private set; } = 0;
    public int GetPlayer()
    {
        var currentMove = MoveCount + 1;
        if (currentMove % 2 == 0)
        {
            return 2;
        }
        else
        {
            return 1;
        }
    }

    public MoveResult Move(GameBoard board, int column)
    {
        var moveResult = new MoveResult()
        {
            BoardState = board,
            IsValid = true,
            ErrorMessage = ""
        };
        var columnMove = column - 1;
        var validation = IsValidColumn(columnMove);
        if (validation == null)
        {
            var row = GetMoveRow(board, columnMove);
            if (row == 100)
            {
                InvalidMoveCount++;
                var dnf = CheckInvalidMoveCount(board);
                if (dnf != 0)
                {
                    return ProcessError("Too many invalid moves game over");                    
                }                
                return ProcessError("Invalid Move: Column is full");
            }
            validation = IsValidMove(board, row, columnMove);
            if (validation == null)
            {
                InvalidMoveCount = 0;
                board = MakeMove(board, row, columnMove);
                LastMove = new Corrdinate { Row = row, Column = columnMove };
                board.Winner = CheckWinner(board);
            }
            else
            {
                InvalidMoveCount++;
                var dnf = CheckInvalidMoveCount(board);
                if (dnf != 0)
                {
                    return ProcessError("Too many invalid moves game over");
                }
                return ProcessError($"Invalid Move: {validation}");
            }
        }
        else
        {            
            return ProcessError($"Invalid Move: {validation}");
        }
        moveResult.BoardState = board;
        return moveResult;

    }


    private MoveResult ProcessError(string errorMessage)
    {
        return new MoveResult
        {
            BoardState = this,
            ErrorMessage = errorMessage,
            IsValid = false
        };
    }

    private int CheckInvalidMoveCount(GameBoard board)
    {
        var winner = 0;
        if (board.InvalidMoveCount >= InvalidMoveCountMax)
        {
            var currentPlayer = GetPlayer();
            if (currentPlayer == 1)
            {
                winner = 2;
            }
            else
            {
                winner = 1;
            }
        }
        board.Winner = winner;
        return winner;
    }



    private static GameBoard MakeMove(GameBoard board, int row, int column)
    {
        board.MoveCount++;
        if (board.MoveCount % 2 == 0)
        {
            board.Player = 2;
        }
        else
        {
            board.Player = 1;
        }
        board.boardState[row, column] = board.Player;
        return board;
    }
    private static int GetMoveRow(GameBoard board, int columnMove)
    {
        //get the row of the move
        for (int i = (board.RowCountMax - 1); i >= 0; i--)
        {
            if (board.boardState[i, columnMove] == 0)
            {
                return i;
            }
        }
        return 100;
    }

    private string? IsValidColumn(int column)
    {
        if (column < 0 || column + 1 > ColumnCountMax)
        {
            return "Column is not valid";
        }
        return null;
    }

    private static string? IsValidMove(GameBoard board, int row, int column)
    {
        if (row < 0 || row + 1 > board.RowCountMax)
        {
            return "Row is not valid";
        }

        if (column < 0 || column + 1 > board.ColumnCountMax)
        {
            return "Column is not valid";
        }

        if (board.boardState[row, column] == 0)
        {
            return null;
        }
        else
        {
            return "Move already taken";
        }
    }

    private int CheckWinner(GameBoard board)
    {
        if (MoveCount > 6)
        {
            for (int row = 0; row < RowCountMax; row++)
            {
                for (int col = 0; col < ColumnCountMax; col++)
                {
                    if (board.boardState[row, col] != 0)
                    {

                        board.Winner = CheckHorizontalWin(board, row, col);

                        if (board.Winner != 0)
                        {
                            return board.Winner;
                        }
                        board.Winner = CheckVerticalWin(board, row, col);
                        if (board.Winner != 0)
                        {
                            return board.Winner;
                        }
                        board.Winner = CheckDiagonalWin(board, row, col);
                        if (board.Winner != 0)
                        {
                            return board.Winner;
                        }
                    }
                }
            }
        }
        return 0;
    }

    private int CheckVerticalWin(GameBoard board, int row, int col)
    {
        var checkPlayer = board.boardState[row, col];
        var currentPlayer = checkPlayer;
        var counter = 0;
        WinningSet = new List<Corrdinate>();
        while (counter < 4 && currentPlayer == checkPlayer && ((row + counter) < RowCountMax))
        {
            currentPlayer = board.boardState[row + counter, col];
            WinningSet.Add(new Corrdinate { Row = row + counter, Column = col });
            counter++;
        }
        if (counter == 4 && currentPlayer == checkPlayer && row + counter <= RowCountMax)
        {
            return checkPlayer;
        }
        WinningSet = null;
        return 0;
    }

    private int CheckHorizontalWin(GameBoard board, int row, int col)
    {
        var checkPlayer = board.boardState[row, col];
        var currentPlayer = checkPlayer;
        var counter = 0;
        WinningSet = new List<Corrdinate>();
        while (counter < 4 && currentPlayer == checkPlayer && ((col + counter) < ColumnCountMax))
        {
            currentPlayer = board.boardState[row, col + counter];
            WinningSet.Add(new Corrdinate { Row = row, Column = col + counter });

            counter++;
        }
        if (counter == 4 && currentPlayer == checkPlayer && col + counter <= ColumnCountMax)
        {
            return checkPlayer;
        }
        WinningSet = null;

        return 0;
    }
    //TODO: Update to fix diagonal win check
    private int CheckDiagonalWin(GameBoard board, int row, int col)
    {
        var checkPlayer = board.boardState[row, col];
        var currentPlayer = checkPlayer;
        var counter = 0;
        //check diagonal down
        WinningSet = new List<Corrdinate>();
        while (counter < 4 && currentPlayer == checkPlayer && (col + counter < ColumnCountMax) && (row + counter < RowCountMax))
        {
            currentPlayer = board.boardState[row + counter, col + counter];
            WinningSet.Add(new Corrdinate { Row = row + counter, Column = col + counter });
            counter++;
        }
        if (counter == 4 && currentPlayer == checkPlayer)
        {
            return checkPlayer;
        }
        //check diagonal up
        WinningSet = new List<Corrdinate>();

        counter = 0;
        while (counter < 4 && currentPlayer == checkPlayer && (col + counter < ColumnCountMax) && (row - counter > 0))
        {
            currentPlayer = board.boardState[row - counter, col + counter];
            WinningSet.Add(new Corrdinate { Row = row - counter, Column = col + counter });

            counter++;
        }
        if (counter == 4 && currentPlayer == checkPlayer)
        {
            return checkPlayer;
        }
        WinningSet = null;

        return 0;
    }

}