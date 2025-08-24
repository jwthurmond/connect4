using connect4.library;
using connect4.tournament;

namespace connect4;

internal class Program
{
    static void Main()
    {
    
        var board = new GameBoard();
        var player1 = PickPlayer(
                new IConnect4Player[]
                {
                    new HumanInput(),
                    new RandomPlayer(),
                    new IncrementBy1(),
                    new Always4(),
                    new Lowest(),
                    new Highest(),
                }
            , "Select Player 1:");
        var player1WinCount = 0;
        var player2 = PickPlayer(
            new IConnect4Player[]
            {
                new HumanInput(),
                new RandomPlayer(),
                new IncrementBy1(),
                new Always4(),
                new Lowest(),
                new Highest(),
            }
            , "Select Player 2:");
        var player2WinCount = 0;
        var drawCount = 0;
        var keepPlaying = true;

        if (player1.Color == player2.Color)
        {
            player1.Color = ConsoleColor.Red;
            player2.Color = ConsoleColor.Yellow;
        }

        while (keepPlaying)
        {
            player1.StartNewGame();
            player2.StartNewGame();
            Console.ForegroundColor = player1.Color;
            Console.Write($"{player1.Name}");
            Console.ResetColor();
            Console.Write(" vs ");
            Console.ForegroundColor = player2.Color;
            Console.WriteLine($"{player2.Name}");
            Console.ResetColor();

            while (board.Winner == 0 && board.MoveCount < board.MaxMoves)
            {
                try
                {
                    IConnect4Player currentPlayer = player1;
                    if (board.GetPlayer() != 1)
                    {
                        currentPlayer = player2;
                    }

                    if (currentPlayer.ShowBoardBeforeMove)
                    {
                        board.PrintToConsole(player1Color: player1.Color, player2Color: player2.Color);
                    }

                    var column = currentPlayer.GetMove(board);
                    var result = board.Move(board, column);
                    board = result.BoardState;
                    if (!result.IsValid)
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
                board.PrintToConsole(player1Color: player1.Color, player2Color: player2.Color);
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
                Console.WriteLine();
                board = new GameBoard();
            }
        }
    }
    
    private static IConnect4Player PickPlayer(IConnect4Player[] playerClasses, string prompt = "Select a player:")
    {
        while(true)
        {
            Console.WriteLine(prompt);
            for (var i = 0; i < playerClasses.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {playerClasses[i].Name}");
            }
            Console.Write("Enter the number of the player: ");
            var input = Console.ReadLine();
            IConnect4Player playerClass;
            if (int.TryParse(input, out var selectedIndex) && selectedIndex > 0 && selectedIndex <= playerClasses.Length)
            {
                playerClass =  playerClasses[selectedIndex - 1];
                if (playerClass.AcceptsCustomName)
                {
                    Console.Write("Enter your name: ");
                    var name = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(input))
                    {
                         playerClass.Name = name ?? "No input";
                    }
                }

                return playerClass;
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