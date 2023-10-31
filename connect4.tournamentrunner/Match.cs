using connect4.library;

namespace connect4.tournamentrunner
{
    public class Match
    {   
        public Match(int roundsPerMatch, IConnect4Player playerA, IConnect4Player playerB)
        {
            RoundsPerMatch = roundsPerMatch;
            PlayerA = playerA;
            PlayerB = playerB;        
        }
        public List<GameBoard> Games { get; private set; } = new List<GameBoard>();
        public IConnect4Player PlayerA { get; private set; }
        public IConnect4Player PlayerB { get; private set; }        
        public int RoundsPerMatch { get; private set; } = 1;
        public int PlayerAWinCount { get; private set; } = 0;
        public int PlayerBWinCount { get; private set; } = 0;
        public int DrawCount { get; private set; } = 0;
        
        public void RunMatch(bool showBoardAfterEachRound)
        {
            for(int i = 0; i < RoundsPerMatch; i++)
            {
                GameBoard board = new GameBoard();
                while (board.Winner == 0 && board.MoveCount < board.MaxMoves)
                {
                    try
                    {
                        var column = 0;
                        if (board.GetPlayer() == 1)
                        {
                            column = PlayerA.GetMove(board);
                        }
                        else
                        {
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
                        Console.WriteLine(err.Message);
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
                if (showBoardAfterEachRound)
                {
                    //show the board
                }
                Games.Add(board);
            }
        }

    }
}
