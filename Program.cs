using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku
{
    class Program
    {
        static void Main(string[] args)
        {
            Game g = new Game();
            Console.ReadLine();
        }
    }

    class Game
    {
        const int difficulty = 4; //edit this variable for defining the probability of clear values
        int[,] grid = new int[9, 9];
        Random r = new Random();
        int cycles = 0;
        int calls = 0;

        public Game()
        {
            GenerateRecursive(0, 0);
            RenderGrid();
        }

        private void RenderGrid()
        {
            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    if (r.Next(0, difficulty) != 0)
                    {
                        Console.Write("{0,-4}", " " + grid[x, y]);
                    }
                    else
                    {
                        Console.Write("{0,-4}", " ");
                    }

                    if ((x + 1) % 3 == 0)
                    {
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.Write("  ");
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                }
                Console.Write("\r\n");
                if ((y + 1) % 3 == 0)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    for (int i = 0; i < 42; i++)
                    {
                        Console.Write(" ");
                    }
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                Console.Write("\r\n");
            }
            Console.WriteLine("For Cycles: " + cycles + "\tRecursive Calls: " + calls + "\r\nBacktrack count: " + (calls - 81));
        }

        private bool GenerateRecursive(int x, int y)
        {
            calls++;
            for (int i = 1; i <= 9; i++)
            {
                bool block = CheckBlockValue(x / 3, y / 3, i);
                bool row = CheckRow(y, i);
                bool column = CheckColumn(x, i);
                if (block == true && row == true && column == true)
                {
                    grid[x, y] = i;
                    if (x == 8 && y == 8)
                    {
                        return true;
                    }
                    Tuple<int, int> nextPosition = GetNextPosition(x, y);
                    bool nextIteration = GenerateRecursive(nextPosition.Item1, nextPosition.Item2);
                    if (nextIteration == true)
                    {
                        return true;
                    }
                    else
                    {
                        continue;
                    }
                }
                cycles++;
            }
            grid[x, y] = 0;
            return false;
        }

        private Tuple<int, int> GetNextPosition(int x, int y)
        {
            if (x != 8)
            {
                return new Tuple<int, int>(x + 1, y);
            }
            else
            {
                return new Tuple<int, int>(0, y + 1);
            }
        }


        private int[] LoadBlockValues(int x, int y)
        {
            int[] block = new int[9];
            int i = 0;
            for (int a = x * 3; a < (x * 3) + 3; a++)
            {
                for (int b = y * 3; b < (y * 3) + 3; b++)
                {
                    block[i++] = grid[a, b];
                }
            }
            return block;
        }

        private bool CheckBlockValue(int x, int y, int value)
        {
            int[] block = LoadBlockValues(x, y);
            if (block.Contains(value))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private int[] LoadRow(int y)
        {
            int[] row = new int[9];
            for (int i = 0; i < 9; i++)
            {
                row[i] = grid[i, y];
            }
            return row;
        }

        private bool CheckRow(int y, int value)
        {
            int[] row = LoadRow(y);
            if (row.Contains(value))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private int[] LoadColumn(int x)
        {
            int[] column = new int[9];
            for (int i = 0; i < 9; i++)
            {
                column[i] = grid[x, i];
            }
            return column;
        }

        private bool CheckColumn(int x, int value)
        {
            int[] column = LoadColumn(x);
            if (column.Contains(value))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private static int[] ReturnUnusedValues(int[] input)
        {
            List<int> possibleSolutions = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            for (int i = 0; i < input.Length; i++)
            {
                possibleSolutions.Remove(input[i]);
            }
            return possibleSolutions.ToArray();
        }
    }
}
