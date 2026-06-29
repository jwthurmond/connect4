using connect4.runtournament;
using connect4.tournament;

var pluginDir = args.Length > 0 ? args[0] : Path.Combine(AppContext.BaseDirectory, "plugins");

var tourny = new Tournament(20);
tourny.AddPlayer(new RandomPlayer(), playerId: 1);
tourny.AddPlayer(new IncrementBy1(), playerId: 2);
tourny.AddPlayer(new Always4(), playerId: 3);
tourny.AddPlayer(new Lowest(), playerId: 4);
tourny.AddPlayer(new Highest(), playerId: 5);

var nextId = 6;
foreach (var player in PlayerLoader.LoadFromDirectory(pluginDir))
{
    tourny.AddPlayer(player, playerId: nextId++);
}

tourny.Run();
tourny.DisplayAllMatchDetails();
tourny.DisplayResults();
