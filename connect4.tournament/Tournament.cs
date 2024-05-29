using connect4.library;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace connect4.tournament
{
    public class Tournament
    {
        public Tournament(int roundsPerMatch, bool debug = false)
        {
            RoundsPerMatch = roundsPerMatch;
            this.debug = debug;
        }
        public bool debug = false;
        public Dictionary<int, IConnect4Player> Players { get; private set; } = new Dictionary<int, IConnect4Player>();

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
                                var match = new Match(RoundsPerMatch, playerId, Players[playerId], otherPlayerId, Players[otherPlayerId]);
                                Matches = Matches.Append(match);
                            }
                        }
                    }
                    playersUsed.Add(playerId);
                }
                var playSelf = new Match(RoundsPerMatch, playerId, Players[playerId], playerId, Players[playerId]);
                Matches = Matches.Append(playSelf);
            }

            foreach (var match in Matches)
            {
                match.RunMatch(showBoardAfterEachRound: debug);
            }
        }
        public IEnumerable<TournamentResult> GetResults()
        {
            var tourneyResults = new List<TournamentResult>();
            foreach (var playerId in Players.Keys)
            {
                var matchWins = 0;
                var gameWins = 0;
                foreach (var match in Matches)
                {
                    if (match.PlayerAId == playerId)
                    {
                        gameWins += match.PlayerAWinCount;
                        if (match.PlayerAWinCount > match.PlayerBWinCount)
                        {
                            matchWins++;
                        }
                    }
                    if (match.PlayerBId == playerId)
                    {
                        gameWins += match.PlayerBWinCount;
                        if (match.PlayerBWinCount > match.PlayerAWinCount)
                        {
                            matchWins++;
                        }
                    }
                }
                tourneyResults.Add(new TournamentResult(playerId: playerId, matchWins: matchWins, gameWins: gameWins));
            }
            return tourneyResults.OrderByDescending(t => t.GameWins);
        }
        public void DisplayResults()
        {
            var results = GetResults();
            var maxNameLen = Players.Values.Max(p => p.Name.Length);
            Console.WriteLine("Results:");
            Console.WriteLine($"{"Player".PadRight(maxNameLen)} {"Game Wins"} {"Match Wins"}");
            foreach (var result in results)
            {
                Console.WriteLine($"{Players[result.PlayerId].Name.PadRight(maxNameLen)} {result.GameWins.ToString().PadRight(9)} {result.MatchWins.ToString().PadRight(10)}");
            }
        }
        public void DisplayAllMatchDetails()
        {
            foreach (var match in Matches)
            {
                DisplayMatchDetails(match);
                Console.WriteLine();
            }
        }
        public void DisplayMatchDetails(Match match)
        {
            var playerAColor = match.PlayerA.Color;
            var playerBColor = match.PlayerB.Color;
            if (match.PlayerA.Color == match.PlayerB.Color || match.PlayerA.Name == match.PlayerB.Name)
            {
                playerAColor = match.PlayerA.Color;
                playerBColor = match.PlayerB.AlternateColor;
            }
            

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
            var blocks = (int)Math.Ceiling((decimal)match.RoundsPerMatch / (decimal)roundsPerLine);
            for (int block = 0; block < blocks; block++)
            {
                for (int row = 0; row <= match.Games.First().RowCountMax; row++)
                {
                    for (int i = 0; i < roundsPerLine; i++)
                    {
                        var game = (block * roundsPerLine) + i;
                        if (game < match.Games.Count)
                        {
                            match.Games[game].PrintRowToConsole(row, player1Color: playerAColor, player2Color: playerBColor);
                            Console.Write("  ");
                        }
                    }
                    Console.Write('\n');
                }
            }
        }
    }


}