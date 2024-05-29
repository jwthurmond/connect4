using connect4.library;
using FluentAssertions;
using Xunit;

namespace connect4.tests;

public class HorizontalTest
{
    [Fact]
    public void TestHorizontalWinRow1Column4To7()
    {
        GameBoard testBoard = new();

        //Arrange
        var expected = 1;
        _ = testBoard.Move(testBoard, 4); //player 1
        _ = testBoard.Move(testBoard, 4); //player 2
        _ = testBoard.Move(testBoard, 5); //player 1
        _ = testBoard.Move(testBoard, 5); //player 2
        _ = testBoard.Move(testBoard, 6); //player 1
        _ = testBoard.Move(testBoard, 6); //player 2
        var boardResult = testBoard.Move(testBoard, 7); //player 1
        boardResult.BoardState.Winner.Should().Be(expected);
    }

    [Fact]
    public void TestHorizontalWinRow1Column1To4()
    {
        GameBoard testBoard = new();

        //Arrange
        var expected = 1;
        _ = testBoard.Move(testBoard, 1); //player 1
        _ = testBoard.Move(testBoard, 1);//player 2
        _ = testBoard.Move(testBoard, 2); //player 1
        _ = testBoard.Move(testBoard, 2); //player 2
        _ = testBoard.Move(testBoard, 3); //player 1
        _ = testBoard.Move(testBoard, 3); //player 2
        var boardResult = testBoard.Move(testBoard, 4); //player 1
        boardResult.BoardState.Winner.Should().Be(expected);
    }


    [Fact]
    public void TestHorizontalWinRow2Column4To7()
    {
        GameBoard testBoard = new();

        //Arrange
         var expected = 2;
        _ = testBoard.Move(testBoard, 4); //player 1
        _ = testBoard.Move(testBoard, 4); //player 2
        _ = testBoard.Move(testBoard, 5); //player 1
        _ = testBoard.Move(testBoard, 5); //player 2
        _ = testBoard.Move(testBoard, 6); //player 1
        _ = testBoard.Move(testBoard, 6); //player 2
        _ = testBoard.Move(testBoard, 1); //player 1
        _ = testBoard.Move(testBoard, 7); //player 2
        _ = testBoard.Move(testBoard, 1); //player 1
        var boardResult = testBoard.Move(testBoard, 7); //player 2
        boardResult.BoardState.Winner.Should().Be(expected);
    }

}