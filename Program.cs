using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace sudoku
{
    class Program
    {
        static void Main(string[] args)
        {
            int[,] gridInput = null;
            string fileOutput = string.Empty;
            int difficulty = 0;

            Console.WriteLine("Made by wortelus in 2018, licensed under MIT License... -h for help");
            for (int i = 0; i < args.Length; i++)
            {
                string currentArg = args[i].Trim().ToLower(); //normalizing input argument

                if (currentArg == "-f")
                {
                    try
                    {
                        gridInput = LoadGridFromString(File.ReadAllText(args[i + 1]));
                    }
                    catch(IndexOutOfRangeException)
                    {
                        Console.WriteLine("You didn't specify the file input.");
                        return;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("There was an error during loading of a file input:\r\n" + ex.Message);
                        return;
                    }
                }
                if (currentArg == "-o")
                {
                    try
                    {
                        fileOutput = args[i + 1];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        Console.WriteLine("You didn't specify the file output.");
                        return;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("There was an error during loading of a file output:\r\n" + ex.Message);
                        return;
                    }
                }
                if (currentArg == "-d")
                {
                    try
                    {
                        difficulty = int.Parse(args[i + 1]);
                    }
                    catch (IndexOutOfRangeException)
                    {
                        Console.WriteLine("You didn't specify the difficulty, setting to 0 (all numbers visible).");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("There was an error during loading of an difficulty, setting to 0 (all numbers visible):\r\n" + ex.Message);
                    }
                }
                if (currentArg == "-h")
                {
                    PrintHelp();
                }
            }

            if (args.Length == 0)
            {
                Console.WriteLine("No arguments specified... generating new random board, for help use -h :");
                Game g = new Game();
            }
            else if (gridInput != null)
            {
                Game g = new Game(gridInput, fileOutput);
            }
            else
            {
                Game g = new Game(difficulty, fileOutput);
            }
        }

        static void PrintHelp()
        {
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine("-f [FILE PATH]\t\t --- specify unsolved board path to solve.");
            Console.WriteLine("-o [FILE PATH]\t\t --- specify path to save generated output board.");
            Console.WriteLine("-h\t\t\t --- this help screen.");
            Console.WriteLine("-d [NUMBER]\t\t --- specify the probability (1/x) of exposing a value\r\n\t\t\t(bigger numbers = harder, less exposed numbers).");
            Console.WriteLine("---------------------------------------------");
        }


        static int[,] LoadGridFromString(string input)
        {
            int[,] output = new int[9,9];
            input.Trim();
            string[] split = input.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            if (split.Length < 9)
            {
                throw new Exception("input file must have 9 lines.");
            }
            for (int i = 0; i < 9; i++)
            {
                if (split[i].Length < 9)
                {
                    throw new Exception("input file must have 9 characters in each line, check line number: " + i);
                }
                for (int j = 0; j < 9; j++)
                {
                    if (Char.IsDigit(split[i][j]))
                    {
                        output[j, i] = split[i][j] - '0';
                    }
                }
            }
            return output;
        }
    }

    class Game
    {
        int difficulty; //edit this variable for defining the probability of clear values
        int[,] grid = new int[9, 9];
        Random r = new Random();
        int cycles = 0;
        int calls = 0;
        int backtracked = 0;
        List<Tuple<int, int>> gridSolveValues = new List<Tuple<int, int>>();

        public Game(int difficulty_ = 0, string outputPath = "")
        {
            difficulty = difficulty_;
            GenerateRandomRecursive(0, 0);
            RenderGrid();
            if (outputPath != "")
            {
                try
                {
                    File.WriteAllText(outputPath, GetSaveString());
                    Console.WriteLine("Generated board succesfully saved to the following file: " + outputPath + "\r\n(empty spaces may have different locations).");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("There was an error during saving a solved board:\r\n" + ex.Message);
                }
            }
        }

        /// <summary>
        /// constructor for solving pre-defined sudoku boards
        /// </summary>
        /// <param name="inputGrid"></param>
        public Game(int[,] inputGrid, string outputPath = "")
        {

            difficulty = 0; //to show completed board
            grid = inputGrid;

            if (IsSolvedGrid())
            {
                Console.WriteLine("Input board is already solved...");
                return;
            }

            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    if (grid[x, y] == 0)
                    {
                        Tuple<int, int> value = new Tuple<int, int>(x, y);
                        gridSolveValues.Add(value);
                    }
                }
            }
            if (!SolveRecursive(gridSolveValues[0].Item1, gridSolveValues[0].Item2))
            {
                Console.WriteLine("Input board is unsolvable...");
            }
            else
            {
                RenderGrid();
                if (outputPath != "")
                {
                    try
                    {
                        File.WriteAllText(outputPath, GetSaveString());
                        Console.WriteLine("Solved board succesfully saved to the following file: " + outputPath);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("There was an error during saving a solved board:\r\n" + ex.Message);
                    }
                }
            }
        }

        private bool IsSolvedGrid()
        {
            for (int a = 0; a < 9; a++)
            {
                for (int b = 0; b < 9; b++)
                {
                    if (grid[a, b] == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private string GetSaveString()
        {
            string output = string.Empty;
            for (int i = 0; i < 9; i++)
            {
                for (int a = 0; a < 9; a++)
                {
                    int value = grid[a, i];
                    if (value == 0)
                    {
                        return null;
                    }
                    if (r.Next(0, difficulty) == 0)
                    {
                        output += value;
                    }
                    else
                    {
                        output += "X";
                    }

                }

                if (i != 8)
                {
                    output += "\r\n";
                }
            }
            return output;
        }

        private bool SolveRecursive(int x, int y)
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
                    Tuple<int, int> nextPosition = GetNextSolvePosition(x, y);
                    if (nextPosition == null)
                    {
                        return true;
                    }
                    bool nextIteration = SolveRecursive(nextPosition.Item1, nextPosition.Item2);
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
            backtracked++;
            grid[x, y] = 0;
            return false;
        }

        private void RenderGrid()
        {
            DrawStraightLine();
            Console.Write("\r\n");
            for (int y = 0; y < 9; y++)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.Write("  ");
                Console.BackgroundColor = ConsoleColor.Black;

                for (int x = 0; x < 9; x++)
                {
                    if (r.Next(0, difficulty) == 0)
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
                    DrawStraightLine();
                }
                Console.Write("\r\n");
            }
            Console.WriteLine("For Loop Iteration Count: " + cycles + "\r\nTotal Recursive Calls: " + calls + "\r\nBacktrack Count: " + backtracked);
        }

        private void DrawStraightLine()
        {
            Console.BackgroundColor = ConsoleColor.Gray;
            for (int i = 0; i < 44; i++)
            {
                Console.Write(" ");
            }
            Console.BackgroundColor = ConsoleColor.Black;
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
            backtracked++;
            grid[x, y] = 0;
            return false;
        }

        private bool GenerateRandomRecursive(int x, int y)
        {
            calls++;
            int start = r.Next(1, 10);
            int end = 9;
            if (start != 1)
            {
                end = start - 1;
            }

            for (int i = start; i != end ; i++)
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
                    bool nextIteration = GenerateRandomRecursive(nextPosition.Item1, nextPosition.Item2);
                    if (nextIteration == true)
                    {
                        return true;
                    }
                    else
                    {
                        if (i == 9 && end != 9)
                        {
                            i = 0;
                        }
                        continue;
                    }
                }
                if (i == 9 && end != 9)
                {
                    i = 0;
                }
                cycles++;
            }
            backtracked++;
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

        private Tuple<int, int> GetNextSolvePosition(int x, int y)
        {
            for (int i = 0; i < gridSolveValues.Count; i++)
            {
                if (i == gridSolveValues.Count - 1)
                {
                    return null;
                }
                else if (gridSolveValues[i].Item1 == x && gridSolveValues[i].Item2 == y)
                {
                    return gridSolveValues[i + 1];
                }
            }
            throw new Exception("couldn't find next position in solve grid, wrong axis coordinations");
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
    }
}
