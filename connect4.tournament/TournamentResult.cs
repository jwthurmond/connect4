namespace connect4.tournament;

public class TournamentResult
{
    public TournamentResult(int playerId, int matchWins, int gameWins) 
    {
        PlayerId = playerId;
        MatchWins = matchWins;
        GameWins = gameWins;
    }
    public int PlayerId { get; init; }
    public int MatchWins { get; init; }
    public int GameWins { get; init; }
}
