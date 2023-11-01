namespace connect4.runtournament
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var tourny = new tournament.Tournament(20);
            tourny.AddPlayer(new tournament.RandomPlayer(), playerId:1);
            tourny.AddPlayer(new tournament.IncrementBy1(), playerId:2);
            tourny.AddPlayer(new tournament.Always4(), playerId:3);
            tourny.AddPlayer(new tournament.Lowest(), playerId:4);
            tourny.Run();
            tourny.DisplayAllMatchDetails();
            tourny.DisplayResults();
        }
    }
}