using connect4.library;

namespace connect4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var board = new GameBoard();
            PrintBoard(board);
            while (board.Winner == 0)
            {
                try
                {
                    Console.Write($"Player {board.GetPlayer()} Enter the Column:");
                    var column = int.Parse(Console.ReadLine());
                    board = board.Move(board, column);
                    PrintBoard(board);
                }
                catch (Exception err)
                {
                    Console.WriteLine(err.Message);
                }
            }
        }

        private static void PrintBoard(GameBoard board)
        {
            //write the current state of the board to the console
            Console.WriteLine(" 1 2 3 4 5 6 7");
            for (int row = 0; row < 6; row++)
            {
                Console.Write("|");
                for (int col = 0; col < 7; col++)
                {
                    Console.Write(board.CurrentBoard[row, col]);
                    Console.Write("|");
                }
                Console.WriteLine();
            }
        }
    }
}