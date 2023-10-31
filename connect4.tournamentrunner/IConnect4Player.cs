using connect4.library;


namespace connect4.tournamentrunner
{
    public interface IConnect4Player
    {
        string Name { get; }
        int GetMove(GameBoard board);
    }

    public class RandomPlayer : IConnect4Player
    {
        public string Name => "Random Player";

        public int GetMove(GameBoard board)
        {
            var random = new Random();
            var column = random.Next(0, board.ColumnCountMax);
            return column;
        }
    }

    public class IncrementBy1:IConnect4Player
    {
        public string Name => "Increment by 1";
        private int _lastMove = 1;
        public int GetMove(GameBoard board)
        {
            return _lastMove++ % board.ColumnCountMax;

        }

    }

    public class Always4:IConnect4Player
    {
        public string Name => "Always 4";
        public int GetMove(GameBoard board)
        {
            return 4;
        }
    }
}
