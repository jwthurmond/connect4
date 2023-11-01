using connect4.library;

namespace connect4
{
    internal class Program
    {
        static void Main()
        {
            var board = new GameBoard();
            var player1WinCount = 0;
            var player2WinCount = 0;
            var drawCount = 0;
            var keepPlaying = true;

            while (keepPlaying)
            {
                board.PrintToConsole();
                while (board.Winner == 0 && board.MoveCount < board.MaxMoves)
                {
                    try
                    {
                        if (board.GetPlayer() == 1)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                        }

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
                            board.PrintToConsole();
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
                if (keepPlayingGame != null && keepPlayingGame.ToLower() == "q")
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
    }
}