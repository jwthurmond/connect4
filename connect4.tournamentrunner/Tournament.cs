using connect4.library;

namespace connect4.tournamentrunner
{
    public class Tournament
    {
        public Tournament(int roundsPerMatch)
        {
            RoundsPerMatch = roundsPerMatch;
        }
        public IEnumerable<IConnect4Player> Players { get; private set; } = new List<IConnect4Player>();

        public int RoundsPerMatch { get; private set; } = 1;
        public IEnumerable<Match> Matches { get; private set; } = new List<Match>();

        public bool AddPlayer(IConnect4Player player)
        {
            if (Players.Any(p => p.Name == player.Name))
            {
                return false;
            }
            Players = Players.Append(player);
            return true;
        }

        public bool RemovePlayer(IConnect4Player player)
        {
            if (Players.All(p => p.Name != player.Name))
            {
                return false;
            }
            Players = Players.Where(p => p.Name != player.Name);
            return true;
        }
        public void Run(bool showBoardAfterEachRound)
        {
            List<string> playersUsed = new List<string>();
            foreach (var function in Players)
            {
                if (playersUsed.Contains(function.Name))
                {
                    continue;
                }
                else
                {
                    foreach (var player in Players)
                    {
                        if (!playersUsed.Contains(player.Name))
                        {
                            if (function.Name != player.Name)
                            {
                                var match = new Match(RoundsPerMatch, function, player);
                                Matches = Matches.Append(match);
                            }
                        }
                    }
                    playersUsed.Add(function.Name);
                }
            }

            foreach (var match in Matches)
            {
                match.RunMatch(showBoardAfterEachRound);
            }
        }
    }


}