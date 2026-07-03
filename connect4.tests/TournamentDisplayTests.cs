using connect4.tournament;
using Xunit;

namespace connect4.tests;

public class TournamentDisplayTests
{
    [Fact]
    public void GetDisplayColors_UsesDistinctColorsForSelfPlayAndChangesByMatchIndex()
    {
        var player = new Always4();
        var selfMatchOne = new Match(roundsPerMatch: 1, playerAId: 1, playerA: player, playerBId: 1, playerB: player);
        var selfMatchTwo = new Match(roundsPerMatch: 1, playerAId: 1, playerA: player, playerBId: 1, playerB: player);

        var (leftColorOne, rightColorOne) = Tournament.GetDisplayColors(selfMatchOne, matchIndex: 0);
        var (leftColorTwo, rightColorTwo) = Tournament.GetDisplayColors(selfMatchTwo, matchIndex: 1);

        Assert.NotEqual(leftColorOne, rightColorOne);
        Assert.NotEqual(leftColorTwo, rightColorTwo);
        Assert.NotEqual(leftColorOne, leftColorTwo);
    }
}
