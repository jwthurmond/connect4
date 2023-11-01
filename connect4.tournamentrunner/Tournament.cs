using connect4.library;
using System.Diagnostics;

namespace connect4.tournamentrunner
{
    public class Tournament
    {
        public Tournament(int roundsPerMatch, bool debug = false)
        {
            RoundsPerMatch = roundsPerMatch;
            this.debug = debug;
        }
        public bool debug = false;
        public Dictionary<int,IConnect4Player> Players { get; private set; } = new Dictionary<int, IConnect4Player>();

        public int RoundsPerMatch { get; private set; } = 1;
        public IEnumerable<Match> Matches { get; private set; } = new List<Match>();

        public bool AddPlayer(IConnect4Player player, int playerId)
        {
            if (Players.ContainsKey(playerId))
            {
                return false;
            }
            Players.Add(playerId, player);
            return true;
        }

        public bool RemovePlayer(int playerId)
        {
            if (!Players.ContainsKey(playerId))
            {
                return false;
            }
            Players.Remove(playerId);
            return true;
        }
        public void Run()
        {
            List<int> playersUsed = new List<int>();
            foreach (var playerId in Players.Keys)
            {
                if (playersUsed.Contains(playerId))
                {
                    continue;
                }
                else
                {
                    foreach (var otherPlayerId in Players.Keys)
                    {
                        if (!playersUsed.Contains(otherPlayerId))
                        {
                            if (playerId != otherPlayerId)
                            {
                                var match = new Match(RoundsPerMatch, Players[playerId], Players[otherPlayerId]);
                                Matches = Matches.Append(match);
                            }
                        }
                    }
                    playersUsed.Add(playerId);
                }
            }

            foreach (var match in Matches)
            {
                match.RunMatch(showBoardAfterEachRound:debug);
            }
        }
        public void DisplayResults()
        {

            Console.WriteLine("Results:");
            Console.WriteLine("ToDo: display list of players sorted by number of wins");

        }
        public void DisplayAllMatchDetails()
        {
            foreach(var match in Matches)
            {
                DisplayMatchDetails(match);
                Console.WriteLine();
            }
        }
        public void DisplayMatchDetails(Match match)
        {
            var playerAColor = ConsoleColor.Red;
            var playerBColor = ConsoleColor.Yellow;

            //Print player names
            var playerAWins = $"[{match.PlayerAWinCount} wins]";
            Console.ForegroundColor = playerAColor;
            Console.Write($"{match.PlayerA.Name.PadRight(playerAWins.Length)}");
            Console.ResetColor();
            Console.Write(" vs ");
            Console.ForegroundColor = playerBColor;
            Console.Write($"{match.PlayerB.Name}\n");
            Console.ResetColor();

            Console.ForegroundColor = playerAColor;
            Console.Write(playerAWins.PadRight(match.PlayerA.Name.Length));
            Console.ResetColor();
            Console.Write("    ");
            Console.ForegroundColor = playerBColor;
            Console.Write($"[{match.PlayerBWinCount} wins]");
            Console.ResetColor();
            if (match.DrawCount > 0)
            {
                Console.Write($" ({match.DrawCount} draws)");
            }
            Console.Write('\n');

            //How many rounds can we fit on one line?
            var roundsPerLine = Console.WindowWidth / (match.Games.First().PrintWidth + 2);
            var blocks = (int) Math.Ceiling( (decimal)match.RoundsPerMatch / (decimal)roundsPerLine);
            for (int block = 0; block < blocks; block++) 
            {
                for (int row = 0; row <= match.Games.First().RowCountMax; row++)
                {
                    for (int i = 0; i < roundsPerLine; i++) 
                    {
                        var game = (block * roundsPerLine) + i;
                        if (game < match.Games.Count)
                        {
                            match.Games[game].PrintRowToConsole(row);
                            Console.Write("  ");
                        }
                    }
                    Console.Write('\n');
                }
            }
        }
    }


}