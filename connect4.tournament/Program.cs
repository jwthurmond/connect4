namespace connect4.tournament
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var tournament = new tournamentrunner.Tournament(3);
            tournament.AddPlayer(new tournamentrunner.RandomPlayer());
            tournament.AddPlayer(new tournamentrunner.IncrementBy1());
            tournament.AddPlayer(new tournamentrunner.Always4());
            tournament.Run(true);
            
            
        }
    }
}