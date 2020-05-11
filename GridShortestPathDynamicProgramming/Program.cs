using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GridShortestPathDynamicProgramming
{
    public class Program
    {
        static void Main(string[] args)
        {
            int userInput;
            Console.WriteLine("Hello! we are going to find a shortest path in a weighted grid from the left most y coordinate ");
            Console.WriteLine("to the right most y coordinate, while The only allowed movements are: right, right+up, or right+down.");
            //Console.WriteLine(");

            do
            {
                // get user input for dimension n:
                Console.Write("\nPlease insert n > 1 integer dimension value for the n*n matrix: ");
                while ((!int.TryParse(Console.ReadLine(), out userInput) || (userInput <= 1)))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid Input");
                    Console.ResetColor();
                    Console.Write("Please insert n > 1 integer dimension value for the n*n matrix: ");
                }

                int n = userInput;
                const int maxWeightValueforEachCell = 9;
                int[,] grid = new int[n, n];
                int[,] optimalPathWeights = new int[n, n];
                int i, j;

                // intialize n * n grid with random weights: 
                Random random = new Random();
                for (i = 0; i < n; i++)
                    for (j = 0; j < n; j++)
                        grid[i, j] = random.Next(maxWeightValueforEachCell + 1);

                // initialize optimal path weights for left most layer cells of the grid: 
                for (j = 0; j < n; j++)
                    optimalPathWeights[0, j] = grid[0, j];

                // initialize optimal path weights for upper most layer cells of the grid: 
                j = 0;
                for (i = 1; i < n; i++)
                    optimalPathWeights[i, j] = grid[i, j] + Math.Min(grid[i - 1, j], grid[i - 1, j + 1]);

                // initialize optimal path weights for most bottom layer cells of the grid: 
                j = n - 1;
                for (i = 1, j = n - 1; i < n; i++)
                    optimalPathWeights[i, j] = grid[i, j] + Math.Min(grid[i - 1, j], grid[i - 1, j - 1]);

                // calculate optimal path weights for all other cells using previously calculated results:
                int minPreviousCellPathWeight;
                for (i = 1; i < n; i++)
                {
                    for (j = 1; j < n - 1; j++)
                    {
                        minPreviousCellPathWeight = Math.Min(optimalPathWeights[i - 1, j - 1], Math.Min(optimalPathWeights[i - 1, j], grid[i - 1, j + 1]));
                        optimalPathWeights[i, j] = grid[i, j] + minPreviousCellPathWeight;
                    }
                }

                // calculate optimal path by finding min path of the right-most layer:
                int minPathWeight = int.MaxValue;
                int PathEndingRowIndex = 0;
                for (j = 0; j < n; j++)
                {
                    if (optimalPathWeights[n - 1, j] <= minPathWeight)
                    {
                        minPathWeight = optimalPathWeights[n - 1, j];
                        PathEndingRowIndex = j;
                    }
                }

                // Calculate shortest path from end to start: 
                int y = PathEndingRowIndex;
                Stack<Point> pathCoordiantesStack = new Stack<Point>();
                pathCoordiantesStack.Push(new Point(n - 1, y));
                for (int x = n - 1; x > 0; x--)
                {
                    if (y == 0)
                    {
                        if (optimalPathWeights[x - 1, y + 1] <= optimalPathWeights[x - 1, y])
                            y = y + 1;
                    }

                    else if (y == n - 1)
                    {
                        if (optimalPathWeights[x - 1, y - 1] <= optimalPathWeights[x - 1, y])
                            y = y - 1;
                    }

                    else
                    {
                        if (optimalPathWeights[x - 1, y - 1] <= optimalPathWeights[x - 1, y])
                            if (optimalPathWeights[x - 1, y - 1] <= optimalPathWeights[x - 1, y + 1])
                                y = y - 1;
                            else
                                y = y + 1;
                        else if (optimalPathWeights[x - 1, y + 1] <= optimalPathWeights[x - 1, y])
                            y = y + 1;
                    }

                    pathCoordiantesStack.Push(new Point(x - 1, y));
                }

                // print grid: 
                Console.WriteLine("The Grid (the path is printed in color:\n");
                for (y = 0; y < n; y++)
                {
                    for (int x = 0; x < n; x++)
                    {
                        if (pathCoordiantesStack.Contains(new Point(x, y)))
                            Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("[{0}]", grid[x, y]);
                        Console.ResetColor();
                    }
                    Console.WriteLine();
                }

                // print shortest path:
                Point point;
                int totalPathWeight = 0;
                Console.Write("\nShortest path is:");
                while (pathCoordiantesStack.Count > 0)
                {
                    point = pathCoordiantesStack.Pop();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("[{0}]", point);
                    Console.ResetColor();
                    if(pathCoordiantesStack.Count > 0)
                        Console.Write("-->");
                    totalPathWeight = totalPathWeight + grid[(int)(point.X), (int)(point.Y)];
                }

                Console.WriteLine("\nTotal path weight: {0}", totalPathWeight);

                Console.WriteLine("\nWant to run application again? (y/n)");

            } while ((String.Compare(Console.ReadLine(), "y", true)) == 0);
        }
    }
}
