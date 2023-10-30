using connect4.library;

namespace connect4
{
    internal class Program
    {
        static void Main()
        {
            var board = new GameBoard();
            var player1WinCount=0;
            var player2WinCount=0;
            var drawCount=0;
            var keepPlaying = true;

            while (keepPlaying)
            {
                PrintBoard(board);
                while (board.Winner == 0 && board.MoveCount < board.MaxMoves)
                {
                    try
                    {
                        if (board.GetPlayer() == 1)
                            Console.ForegroundColor = ConsoleColor.Red;
                        else
                            Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write($"Player {board.GetPlayer()}");
                        Console.ResetColor();
                        Console.Write(" Enter the Column:");
                        var readLine = Console.ReadLine();
                        if (readLine == null)
                        {
                            continue;
                        }
                        var column = int.Parse(readLine);
                        var result = board.Move(board, column);
                        board = result.BoardState;
                        if (result.IsValid)
                        {
                            PrintBoard(board);
                        }
                        else
                        {
                            throw new Exception(result.ErrorMessage);
                        }
                    }
                    catch (Exception err)
                    {
                        Console.WriteLine(err.Message);
                    }
                }
                if (board.Winner != 0)
                {
                    if (board.Winner == 1)
                    {
                        player1WinCount++;
                    }
                    else
                    {
                        player2WinCount++;
                    }
                    Console.WriteLine($"Player {board.Winner} Wins!");
                }
                else
                {
                    drawCount++;
                    Console.WriteLine($"No one wins )-:");
                }
                PrintStats(player1WinCount, player2WinCount, drawCount);
                Console.Write($"Type 'Q' or 'q' to quit:");
                var keepPlayingGame = Console.ReadLine();
                if(keepPlayingGame!=null && keepPlayingGame.ToLower() == "q")
                {
                    keepPlaying = false;
                }
                else
                {
                    board = new GameBoard();
                    Console.Clear();
                }
            }
        }

        private static void PrintStats(int player1WinCount, int player2WinCount, int drawCount)
        {
            Console.WriteLine($"Player 1 Wins: {player1WinCount}");
            Console.WriteLine($"Player 2 Wins: {player2WinCount}");
            Console.WriteLine($"Draws: {drawCount}");
        }

        private static void PrintBoard(GameBoard board)
        {
            //write the current state of the board to the console
            Console.Write("   ");
            for(int i=1;i<=GameBoard.ColumnCountMax;i++)
            {
                Console.Write($"{i} ");
            }
            Console.WriteLine();
            
            for (int row = 0; row < GameBoard.RowCountMax; row++)
            {
                Console.Write($"{row + 1} |");
                for (int col = 0; col < GameBoard.ColumnCountMax; col++)
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
                            if (board.CurrentBoard[row, col] == 1) // player 1
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                            }
                            else // player 2
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                            }
                            if (board.LastMove != null && board.LastMove.Row == row && board.LastMove.Column == col)
                                Console.BackgroundColor = ConsoleColor.DarkGray;
                        }
                        Console.Write(0);
                        Console.ResetColor();
                    }
                    Console.Write("|");
                }
                Console.WriteLine();
            }
        }
    }
}