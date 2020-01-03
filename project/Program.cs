using System;

namespace mineswiffer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("MineSwiffer!");

            while(true)
            {
                Console.WriteLine("New game. Enter minefield size:");
                string sizeStr = Console.ReadLine();
                int size = int.Parse(sizeStr.Trim());
                Console.WriteLine("How many mines?");
                string minesStr = Console.ReadLine();
                int mines = int.Parse(minesStr.Trim());
                var result = Play(size, mines);

                if (result == GameStatusEnum.Lost)
                {
                    Console.WriteLine("LOOOOOOSER!");
                }
                else
                {
                    Console.WriteLine("Winner winner chicken dinner!");
                }
                Console.WriteLine("Play again? (y/n):");
                var choice = Console.ReadKey();
                if (!choice.Key.ToString().Equals("y", StringComparison.InvariantCultureIgnoreCase))
                {
                    break;
                }
            }
            Console.WriteLine();
            Console.WriteLine("Bye now.");
        }

        private static GameStatusEnum Play(int size, int numMines)
        {
            Board brd = Board.Create(size, size, numMines);
            GameStatusEnum status = GameStatusEnum.InProgress;

            while (status == GameStatusEnum.InProgress)
            {
                MoveResultEnum moveResult = NextTurn(brd);
                if (moveResult == MoveResultEnum.Invalid)
                {
                    Console.WriteLine("Invalid coordinates. Try again, but with more effort.");
                }
                else if (moveResult == MoveResultEnum.FieryDeath)
                {
                    status = GameStatusEnum.Lost;
                }
                else
                {
                    if (brd.MinesRemaining == 0)
                    {
                        status = GameStatusEnum.Won;
                    }
                }

            }

            return status;
        }

        private static MoveResultEnum NextTurn(Board brd)
        {
            brd.Print(Console.Out);
            Console.WriteLine($"{brd.MinesRemaining} mines remain!");
            Console.WriteLine("Make your move- enter coordinates as: row,col.");
            string line = Console.ReadLine();
            var coords = line.Split(",");
            int row = int.Parse(coords[0]);
            int col = int.Parse(coords[1]);

            return brd.MakeMove(row, col);
        }
    }
}
