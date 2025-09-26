using connect4.library;
using Xunit;

namespace connect4.tests;

public class BoardDimensionTests
{
    [Fact]
    public void TestColumnValueNegativeRejected()
    {
        GameBoard testBoard = new GameBoard();

        //Arrange
        try { _ = testBoard.Move(testBoard, 0); }
        catch (Exception ex)
        {
            Assert.Contains("Column is not valid", ex.Message);
        }
        
    }

    [Fact]
    public void TestColumnUsedUp()
    {
        GameBoard testBoard = new GameBoard();
        _ = testBoard.Move(testBoard, 1);
        _ = testBoard.Move(testBoard, 1);
        _ = testBoard.Move(testBoard, 1);
        _ = testBoard.Move(testBoard, 1);
        _ = testBoard.Move(testBoard, 1);
        _ = testBoard.Move(testBoard, 1);
       
        //Arrange
        try { _ = testBoard.Move(testBoard, 1); }
        catch (Exception ex)
        {
            Assert.Contains("Column is full", ex.Message);
        }
        
    }

    [Fact]
    public void TestEightByNineInvalidWidth()
    {
        var width = 8;
        var height = 9;

        GameBoard testBoard = new GameBoard(width: width, height: height);

        var result = testBoard.Move(testBoard, 1);
        Assert.True(result.IsValid);

        result = testBoard.Move(testBoard, width);
        Assert.True(result.IsValid);

        result = testBoard.Move(testBoard, 0);
        Assert.False(result.IsValid);
        Assert.Contains("Column is not valid", result.ErrorMessage);

        result = testBoard.Move(testBoard, width+1);
        Assert.False(result.IsValid);
        Assert.Contains("Column is not valid", result.ErrorMessage);
    }

    [Fact]
    public void TestEightByNineColumnUsedUp()
    {
        var width = 8;
        var height = 9;

        GameBoard testBoard = new GameBoard(width:width, height:height);

        for (int i = 0; i < height; i++)
        {
            var moveResult = testBoard.Move(testBoard, 1);
            Assert.True(moveResult.IsValid);
        }

        var result = testBoard.Move(testBoard, 1);
        Assert.False(result.IsValid);
        Assert.Contains("Column is full", result.ErrorMessage);
    }
}
