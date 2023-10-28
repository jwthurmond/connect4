using connect4.library;

namespace connect4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var board = new Board();
            PrintBoard(board);
            while (board.winner == 0)
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

        private static void PrintBoard(Board board)
        {
            //write the current state of the board to the console
            Console.WriteLine(" 1 2 3 4 5 6 7");
            for (int i = 0; i < 6; i++)
            {
                Console.Write("|");
                for (int j = 0; j < 7; j++)
                {
                    Console.Write(board.board[i, j]);
                    Console.Write("|");
                }
                Console.WriteLine();
            }
        }
    }
}