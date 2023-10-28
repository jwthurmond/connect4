namespace connect4.library;
public class GameBoard
{
    public GameBoard()
    {
        //initialize the board
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 7; j++)
            {

                boardState[i, j] = 0;
            }
        }
    }
    private int[,] boardState = new int[6, 7];
    public int[,] CurrentBoard 
    {
        get 
        {
            return boardState;
        }
    }
    public int MoveCount { get; private set; } = 0;
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
        if (row < 0 || row > 6)
        {
            return "Row is not valid";
        }
        if (column < 0 || column > 7)
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
            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    //Console.Write(board.CurrentBoard[row, col]);
                }
            }
        //check for a winner
        board.Winner =0;
    }

    public void CheckBoardForFourInARow()
    {
        //check the board for four in a row

    }

}