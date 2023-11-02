using connect4.library;

namespace connect4.tournament
{
    public class Match
    {
        public Match(int roundsPerMatch, int playerAId, IConnect4Player playerA, int playerBId, IConnect4Player playerB)
        {
            RoundsPerMatch = roundsPerMatch;
            PlayerAId = playerAId;
            PlayerA = playerA;
            PlayerBId = playerBId;
            PlayerB = playerB;
        }
        public List<GameBoard> Games { get; private set; } = new List<GameBoard>();
        public int PlayerAId { get; init; }
        public IConnect4Player PlayerA { get; private set; }
        public int PlayerBId { get; init; }
        public IConnect4Player PlayerB { get; private set; }
        public int RoundsPerMatch { get; private set; } = 1;
        public int PlayerAWinCount { get; private set; } = 0;
        public int PlayerBWinCount { get; private set; } = 0;
        public int DrawCount { get; private set; } = 0;
        public List<string> ErrorList = new List<string>();

        public void RunMatch(bool showBoardAfterEachRound)
        {
            for (int i = 0; i < RoundsPerMatch; i++)
            {
                var currentPlayerName = "";
                GameBoard board = new GameBoard();
                PlayerA.StartNewGame();
                PlayerB.StartNewGame();
                while (board.Winner == 0 && board.MoveCount < board.MaxMoves)
                {
                    try
                    {
                        
                        var column = 0;
                        if (board.GetPlayer() == 1)
                        {
                            currentPlayerName = PlayerB.Name;
                            column = PlayerA.GetMove(board);
                        }
                        else
                        {
                            currentPlayerName = PlayerA.Name;
                            column = PlayerB.GetMove(board);
                        }
                        var result = board.Move(board, column);
                        board = result.BoardState;
                        if (!result.IsValid)
                        {
                            throw new Exception(result.ErrorMessage);
                        }
                    }
                    catch (Exception err)
                    {
                        ErrorList.Add(currentPlayerName + ":" + err.Message);
                    }
                }
                if (board.Winner != 0)
                {
                    if (board.Winner == 1)
                    {
                        PlayerAWinCount++;
                    }
                    else
                    {
                        PlayerBWinCount++;
                    }
                }
                else
                {
                    DrawCount++;
                }
                Games.Add(board);
            }

            if (showBoardAfterEachRound)
            {
                Console.WriteLine("-----------------------------------------------------------------------------");
                Console.WriteLine($"{PlayerA.Name} Win Count: {PlayerAWinCount}");
                Console.WriteLine($"{PlayerB.Name} Win Count: {PlayerBWinCount}");
                Console.WriteLine($"Draw Count: {DrawCount}");
                if (ErrorList.Any())
                {
                    Console.WriteLine("Errors:");
                    foreach (var error in ErrorList)
                    {
                        Console.WriteLine(error);
                    }   
                }
            }
        }

    }
}
