using connect4.library;

namespace connect4
{
    internal class Program
    {
        static void Main()
        {
            var board = new GameBoard();
            PrintBoard(board);
            while (board.Winner == 0 && board.MoveCount < board.MaxMoves)
            {
                try
                {
                    Console.Write($"Player {board.GetPlayer()} Enter the Column:");
                    var readLine = Console.ReadLine();
                    if (readLine == null)
                    {
                        continue;
                    }
                    var column = int.Parse(readLine);
                    board = board.Move(board, column);
                    PrintBoard(board);
                }
                catch (Exception err)
                {
                    Console.WriteLine(err.Message);
                }
            }
            if(board.Winner!=0)
            {
                Console.WriteLine($"Player {board.Winner} Wins!");
            }
            else
            {
                Console.WriteLine($"No one wins )-:");
            }
        }

        private static void PrintBoard(GameBoard board)
        {
            //write the current state of the board to the console
            Console.WriteLine("   1 2 3 4 5 6 7");
            for (int row = 0; row < 6; row++)
            {
                Console.Write($"{row + 1} |");
                for (int col = 0; col < 7; col++)
                {
                    if (board.CurrentBoard[row,col]==0)
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                        if(board.WinningSet!=null && board.WinningSet.Exists(c=>c.Row==row && c.Column == col))
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                        }
                        else
                        {
                            if (board.LastMove != null && board.LastMove.Row == row && board.LastMove.Column == col)
                            {
                                Console.ForegroundColor = ConsoleColor.Blue;
                            }
                            else
                            {
                                Console.ForegroundColor = board.CurrentBoard[row, col] == 1 ? ConsoleColor.Red : ConsoleColor.Yellow;
                            }
                        }
                        Console.Write(board.CurrentBoard[row, col]);
                        Console.ResetColor();
                    }
                    Console.Write("|");
                }
                Console.WriteLine();
            }
        }
    }
}