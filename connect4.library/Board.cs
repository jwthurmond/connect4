namespace connect4.library;
public class GameBoard
{
    public class Corrdinate
    {
        public int Row { get; set; }
        public int Column { get; set; }
    }
    public GameBoard()
    {
        //initialize the board
        for (int i = 0; i < RowMax; i++)
        {
            for (int j = 0; j < ColumnMax; j++)
            {

                boardState[i, j] = 0;
            }
        }
    }

    public List<Corrdinate>? WinningSet { get; set; }

    private int[,] boardState = new int[RowMax, ColumnMax];
    public int[,] CurrentBoard 
    {
        get 
        {
            return boardState;
        }
    }
    public const int RowMax = 6;
    public const int ColumnMax = 7;
    public int MoveCount { get; private set; } = 0;
    public int MaxMoves { get; private set; } = RowMax * ColumnMax;
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

    public GameBoard Move(GameBoard board, int column)
    {
        //TODO: update to return move result instead of board
        var columnMove = column - 1;
        var row = GetMoveRow(board, columnMove);
        var validation = IsValidMove(board, row, columnMove);
        if (validation == null)
        {
            board = MakeMove(board, row, columnMove);
            CheckWinner(board);
        }
        else
        {
            //TODO: update move result instead of throwing exception
            throw new InvalidOperationException($"Invalid Move: {validation}");
        }
        return board;

    }
    private GameBoard MakeMove(GameBoard board, int row, int column)
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
    private int GetMoveRow(GameBoard board, int columnMove)
    {
        //get the row of the move
        for (int i = 5; i >= 0; i--)
        {
            if (board.boardState[i, columnMove] == 0)
            {
                return i;
            }
        }
        return 100;
    }

    private string? IsValidMove(GameBoard board, int row, int column)
    {
        if (row < 0 || row > RowMax)
        {
            return "Row is not valid";
        }
        if (column < 0 || column > ColumnMax)
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

    private void CheckWinner(GameBoard board)
    {
        /*
          
          Horizontal Win Patterns
          row,0|row,1|row,2|row,3
          row,1|row,2|row,3|row,4
          row,2|row,3|row,4|row,5
          row,3|row,4|row,5|row,6

          Verticle Win Patterns
          0,col|1,col|2,col|3,col
          1,col|2,col|3,col|4,col
          2,col|3,col|4,col|5,col

          Horizontal Win Patterns
          0,0|1,1|2,2|3,3
          1,1|2,2|3,3|4,4
          2,2|3,3|4,4|5,5
          0,1|1,2|2,3|3,4
          1,2|2,3|3,4|4,5
          2,3|3,4|4,5|5,5



         */
        if (MoveCount > 6)
        {
            for (int row = 0; row < RowMax; row++)
            {
                for (int col = 0; col < ColumnMax; col++)
                {
                    if (board.boardState[row, col] != 0)
                    {
                        board.Winner = CheckHorizontalWin(board, row, col);

                        if (board.Winner != 0)
                        {
                            return;
                        }
                        board.Winner = CheckVerticalWin(board, row, col);
                        if (board.Winner != 0)
                        {
                            return;
                        }
                        board.Winner = CheckDiagonalWin(board, row, col);
                        if (board.Winner != 0)
                        {
                            return;
                        }
                    }
                }
            }
        }
    }

    private int CheckVerticalWin(GameBoard board, int row, int col)
    {
        var checkPlayer = board.boardState[row, col];
        var currentPlayer = checkPlayer;
        var counter = 0;
        WinningSet = new List<Corrdinate>();
        while (counter < 4 && currentPlayer == checkPlayer && (row + counter <= RowMax))
        {
            currentPlayer = board.boardState[row + counter, col];
            WinningSet.Add(new Corrdinate { Row = row + counter, Column = col });
            counter++;
        }
        if (counter == 4 && currentPlayer == checkPlayer && row + counter <= RowMax)
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
        while(counter < 4 && currentPlayer == checkPlayer && (col + counter <= ColumnMax))
        {
            currentPlayer = board.boardState[row, col + counter];
            WinningSet.Add(new Corrdinate { Row = row , Column = col + counter });

            counter++;
        }
        if (counter == 4 && currentPlayer==checkPlayer && col + counter <= ColumnMax)
        {
            return checkPlayer;
        }
        WinningSet = null;

        return 0;
    }

    private int CheckDiagonalWin(GameBoard board, int row, int col)
    {
        var checkPlayer = board.boardState[row, col];
        var currentPlayer = checkPlayer;
        var counter = 0;
        //check diagonal down
        WinningSet = new List<Corrdinate>();
        while (counter < 4 && currentPlayer == checkPlayer && (col + counter < ColumnMax) && (row + counter < RowMax))
        {
            currentPlayer = board.boardState[row + counter, col + counter];
            WinningSet.Add(new Corrdinate { Row = row + counter, Column = col + counter});
            counter++;
        }
        if (counter == 4 && currentPlayer == checkPlayer)
        {
            return checkPlayer;
        }
        //check diagonal up
        WinningSet = new List<Corrdinate>();

        counter = 0;
        while (counter < 4 && currentPlayer == checkPlayer && (col + counter < ColumnMax) && (row - counter > 0))
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