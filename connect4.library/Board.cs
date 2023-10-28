using System.ComponentModel;

namespace connect4.library
{
    public class Board
    {
        public Board()
        {
            //initialize the board
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 7; j++)
                {

                    board[i, j] = 0;
                }
            }
        }
        public int[,] board = new int[6, 7];
        public int[] column = new int[7];
        public int[] row = new int[6];
        public int[] diag = new int[12];
        public int[] rdiag = new int[12];
        public int[] moves = new int[42];
        public int MoveCount { get; set; } = 0;
        public int Player { get; set; } = 1;
        public int winner = 0;
        public int move = 0;
        public int moveRow = 0;
        public int moveCol = 0;
        public int moveDiag = 0;
        public int moveRDiag = 0;
        public int moveCountCol = 0;
        public int moveCountRow = 0;
        public int moveCountDiag = 0;
        public int moveCountRDiag = 0;
        public int moveCountCol1 = 0;
        public int moveCountRow1 = 0;
        public int moveCountDiag1 = 0;
        public int moveCountRDiag1 = 0;
        public int moveCountCol2 = 0;
        public int moveCountRow2 = 0;
        public int moveCountDiag2 = 0;
        public int moveCountRDiag2 = 0;
        public int moveCountCol3 = 0;
        public int moveCountRow3 = 0;
        public int moveCountDiag3 = 0;
        public int moveCountRDiag3 = 0;
        public int moveCountCol4 = 0;
        public int moveCountRow4 = 0;
        public int moveCountDiag4 = 0;
        public int moveCountRDiag4 = 0;
        public int moveCountCol5 = 0;
        public int moveCountRow5 = 0;
        public int moveCountDiag5 = 0;
        public int moveCountRDiag5 = 0;
        public int moveCountCol6 = 0;
        public int moveCountRow6 = 0;
        public int moveCountDiag6 = 0;
        public int moveCountRDiag6 = 0;
        public int moveCountCol7 = 0;
        public int moveCountRow7 = 0;
        public int moveCountDiag7 = 0;

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

        public Board Move(Board board, int column)
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
        private Board MakeMove(Board board, int row, int column)
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
            board.board[row, column] = board.Player;
            return board;
        }
        private int GetMoveRow(Board board, int columnMove)
        {
            //get the row of the move
            for (int i = 5; i >= 0; i--)
            {
                if (board.board[i, columnMove] == 0)
                {
                    board.moveRow = i;
                    break;
                }
            }
            return board.moveRow;
        }

        private string? IsValidMove(Board board, int row, int column)
        {
            if (row < 0 || row > 6)
            {
                return "Row is not valid";
            }
            if (column < 0 || column > 7)
            {
                return "Column is not valid";
            }

            if (board.board[row, column] == 0)
            {
                return null;
            }
            else
            {
                return "Move already taken";
            }
        }

        private void CheckWinner(Board board)
        {
            //check for a winner
            if (board.moveCountCol == 4 || board.moveCountRow == 4 || board.moveCountDiag == 4 || board.moveCountRDiag == 4)
            {
                board.winner = board.Player;
            }
        }

    }
}