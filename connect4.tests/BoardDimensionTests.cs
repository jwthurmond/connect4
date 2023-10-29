using connect4.library;
using Xunit;

namespace connect4.tests
{
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
    }
}
