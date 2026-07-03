using connect4.library;

namespace connect4.tournament;

public class Match
{
    private readonly bool? _initialPlayerAGoesFirst;

    public Match(int roundsPerMatch, int playerAId, IConnect4Player playerA, int playerBId, IConnect4Player playerB, bool? initialPlayerAGoesFirst = null)
    {
        RoundsPerMatch = roundsPerMatch;
        PlayerAId = playerAId;
        PlayerA = playerA;
        PlayerBId = playerBId;
        PlayerB = playerB;
        _initialPlayerAGoesFirst = initialPlayerAGoesFirst;
    }
    public List<GameBoard> Games { get; private set; } = new List<GameBoard>();
    public List<bool> PlayerAStartedGame { get; private set; } = new List<bool>();
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
        var random = new Random();
        // Randomly assign who goes first: true = PlayerA is game-player 1
        bool playerAGoesFirst = _initialPlayerAGoesFirst ?? (random.Next(2) == 0);

        for (int i = 0; i < RoundsPerMatch; i++)
        {
            PlayerAStartedGame.Add(playerAGoesFirst);
            var currentPlayerName = "";
            GameBoard board = new GameBoard();
            PlayerA.StartNewGame();
            PlayerB.StartNewGame();
            while (board.Winner == 0 && board.MoveCount < board.MaxMoves)
            {
                try
                {
                    var column = 0;
                    bool isGamePlayer1Turn = board.GetPlayer() == 1;
                    IConnect4Player current  = (isGamePlayer1Turn == playerAGoesFirst) ? PlayerA : PlayerB;
                    IConnect4Player opponent = (isGamePlayer1Turn == playerAGoesFirst) ? PlayerB : PlayerA;
                    currentPlayerName = current.Name;
                    column = current.GetMove(board);
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
                // Translate game-player winner back to PlayerA/PlayerB
                bool playerAWon = (board.Winner == 1) == playerAGoesFirst;
                if (playerAWon)
                {
                    PlayerAWinCount++;
                    // Winner goes second next round
                    playerAGoesFirst = false;
                }
                else
                {
                    PlayerBWinCount++;
                    playerAGoesFirst = true;
                }
            }
            else
            {
                DrawCount++;
                // On a draw, flip who goes first
                playerAGoesFirst = !playerAGoesFirst;
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
