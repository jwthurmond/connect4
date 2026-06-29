using connect4.library;
using Xunit;

namespace connect4.tests;

public class DiagonalTests
{
    // Board coordinates: row 0 = top, row (RowCountMax-1) = bottom.
    // Pieces fall to the lowest empty row in a column.
    // Moves alternate P1 (odd moves) and P2 (even moves).

    /// <summary>
    /// Diagonal down-right win for P1: pieces at (2,0),(3,1),(4,2),(5,3).
    ///
    /// Build-up:
    ///   P1 sacrifices into cols 5,6,7 to let P2 stack cols 1,2,3 as needed.
    ///   Move sequence: 5,1,6,1,7,1,  1,2,5,2,  2,3,3,6,4
    ///     col1 gets 3×P2 → (5,0),(4,0),(3,0); then P1→(2,0)
    ///     col2 gets 2×P2 → (5,1),(4,1);        then P1→(3,1)
    ///     col3 gets 1×P2 → (5,2);               then P1→(4,2)
    ///     col4 empty → P1→(5,3)
    /// </summary>
    [Fact]
    public void DiagonalDownRightWin_Player1()
    {
        var board = new GameBoard();
        var expected = 1;

        _ = board.Move(board, 5); // P1 sacrifice
        _ = board.Move(board, 1); // P2 → (5,0)
        _ = board.Move(board, 6); // P1 sacrifice
        _ = board.Move(board, 1); // P2 → (4,0)
        _ = board.Move(board, 7); // P1 sacrifice
        _ = board.Move(board, 1); // P2 → (3,0)
        _ = board.Move(board, 1); // P1 → (2,0) ✓
        _ = board.Move(board, 2); // P2 → (5,1)
        _ = board.Move(board, 5); // P1 sacrifice
        _ = board.Move(board, 2); // P2 → (4,1)
        _ = board.Move(board, 2); // P1 → (3,1) ✓
        _ = board.Move(board, 3); // P2 → (5,2)
        _ = board.Move(board, 3); // P1 → (4,2) ✓
        _ = board.Move(board, 6); // P2 sacrifice
        var result = board.Move(board, 4); // P1 → (5,3) ✓ WIN

        Assert.Equal(expected, result.BoardState.Winner);
    }

    /// <summary>
    /// Diagonal up-right win for P1: pieces at (5,0),(4,1),(3,2),(2,3).
    /// This is the standard case and should work before and after the fix.
    /// </summary>
    [Fact]
    public void DiagonalUpRightWin_Player1()
    {
        var board = new GameBoard();
        var expected = 1;

        _ = board.Move(board, 1); // P1 → (5,0) ✓
        _ = board.Move(board, 2); // P2 → (5,1)
        _ = board.Move(board, 2); // P1 → (4,1) ✓
        _ = board.Move(board, 3); // P2 → (5,2)
        _ = board.Move(board, 5); // P1 sacrifice
        _ = board.Move(board, 3); // P2 → (4,2)
        _ = board.Move(board, 3); // P1 → (3,2) ✓
        _ = board.Move(board, 4); // P2 → (5,3)
        _ = board.Move(board, 6); // P1 sacrifice
        _ = board.Move(board, 4); // P2 → (4,3)
        _ = board.Move(board, 7); // P1 sacrifice
        _ = board.Move(board, 4); // P2 → (3,3)
        var result = board.Move(board, 4); // P1 → (2,3) ✓ WIN

        Assert.Equal(expected, result.BoardState.Winner);
    }

    /// <summary>
    /// Diagonal up-right win that reaches row 0: P1 at (3,0),(2,1),(1,2),(0,3).
    /// Uses a 4×4 board. Exposed the "> 0" bug (should be ">= 0") — the check
    /// stopped before reading (0,3) because row-counter==0 failed the old guard.
    /// </summary>
    [Fact]
    public void DiagonalUpRightWin_ReachingRow0_Player1()
    {
        var board = new GameBoard(4, 4);
        var expected = 1;

        // Stack col4 with 3×P2 so P1 can later land at (0,3).
        // P1 sacrifices into col2 and col3 to keep turns moving.
        //
        // After all setup:
        //   (3,0)=P1, (2,1)=P1, (1,2)=P1, (0,3)=P1
        //
        // Move sequence:
        //   1.P1:col2→(3,1)  2.P2:col4→(3,3)
        //   3.P1:col1→(3,0)✓ 4.P2:col4→(2,3)
        //   5.P1:col2→(2,1)✓ 6.P2:col4→(1,3)
        //   7.P1:col3→(3,2)  8.P2:col3→(2,2)
        //   9.P1:col3→(1,2)✓ 10.P2:col1→(2,0)
        //  11.P1:col4→(0,3)✓ WIN

        _ = board.Move(board, 2); // P1 → (3,1) filler
        _ = board.Move(board, 4); // P2 → (3,3)
        _ = board.Move(board, 1); // P1 → (3,0) ✓
        _ = board.Move(board, 4); // P2 → (2,3)
        _ = board.Move(board, 2); // P1 → (2,1) ✓
        _ = board.Move(board, 4); // P2 → (1,3)
        _ = board.Move(board, 3); // P1 → (3,2) filler
        _ = board.Move(board, 3); // P2 → (2,2)
        _ = board.Move(board, 3); // P1 → (1,2) ✓
        _ = board.Move(board, 1); // P2 → (2,0)
        var result = board.Move(board, 4); // P1 → (0,3) ✓ WIN

        Assert.Equal(expected, result.BoardState.Winner);
    }

    /// <summary>
    /// Diagonal up-right win where the diagonal-down direction from the starting
    /// cell runs into an opponent's piece. Exposed the missing "currentPlayer = checkPlayer"
    /// reset — the up-check used to inherit the opponent's value and short-circuit.
    ///
    /// P1 wins at (5,0),(4,1),(3,2),(2,3).
    /// From (5,0), diagonal-down would check (6,1) which is out of bounds (fine here),
    /// but from (4,1) diagonal-down hits (5,2)=P2, leaving currentPlayer=P2 for the
    /// up check — causing it to miss the win originating from (4,1).
    /// The fix ensures (3,0) correctly detects the full diagonal.
    /// </summary>
    [Fact]
    public void DiagonalUpRightWin_AfterDiagonalDownHitsOpponent_Player1()
    {
        var board = new GameBoard();
        var expected = 1;

        // Same final positions as DiagonalUpRightWin_Player1 but P2 also occupies
        // (5,2), ensuring that CheckDiagonalWin called at (4,1) encounters P2 on
        // the diagonal-down path before finding the up-right win.
        //
        // Move sequence builds P1 win at (5,0),(4,1),(3,2),(2,3)
        // with P2 at (5,2) to trigger the currentPlayer reset bug.
        //
        //  1.P1:col1→(5,0)✓  2.P2:col2→(5,1)
        //  3.P1:col2→(4,1)✓  4.P2:col3→(5,2)  ← P2 sits on diagonal-down from (4,1)
        //  5.P1:col5→sac      6.P2:col3→(4,2)
        //  7.P1:col3→(3,2)✓  8.P2:col4→(5,3)
        //  9.P1:col6→sac     10.P2:col4→(4,3)
        // 11.P1:col7→sac     12.P2:col4→(3,3)
        // 13.P1:col4→(2,3)✓  WIN

        _ = board.Move(board, 1); // P1 → (5,0) ✓
        _ = board.Move(board, 2); // P2 → (5,1)
        _ = board.Move(board, 2); // P1 → (4,1) ✓
        _ = board.Move(board, 3); // P2 → (5,2)  ← triggers currentPlayer bug at (4,1)
        _ = board.Move(board, 5); // P1 sacrifice
        _ = board.Move(board, 3); // P2 → (4,2)
        _ = board.Move(board, 3); // P1 → (3,2) ✓
        _ = board.Move(board, 4); // P2 → (5,3)
        _ = board.Move(board, 6); // P1 sacrifice
        _ = board.Move(board, 4); // P2 → (4,3)
        _ = board.Move(board, 7); // P1 sacrifice
        _ = board.Move(board, 4); // P2 → (3,3)
        var result = board.Move(board, 4); // P1 → (2,3) ✓ WIN

        Assert.Equal(expected, result.BoardState.Winner);
    }
}
