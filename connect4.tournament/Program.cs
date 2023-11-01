namespace connect4.tournament
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var tournament = new tournamentrunner.Tournament(20);
            tournament.AddPlayer(new tournamentrunner.RandomPlayer(), playerId:1);
            tournament.AddPlayer(new tournamentrunner.IncrementBy1(), playerId:2);
            tournament.AddPlayer(new tournamentrunner.Always4(), playerId:3);
            tournament.Run();
            tournament.DisplayAllMatchDetails();
            tournament.DisplayResults();
        }
    }
}