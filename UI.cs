using System.Diagnostics;
using Soduko_Omega.Ants;
using Soduko_Omega.IO;
using Soduko_Omega.Exceptions;
namespace Soduko_Omega;

public static class UI
{
    public static void Game()
    {
        while (true)
        {
            var stopwatch = new Stopwatch();
            Console.WriteLine("Press 1 to take the input from the console");
            Console.WriteLine("Press 2 to take the input from the file");
            Console.WriteLine("Press 3 to exit the program");
            var input = Console.ReadLine();
            if (input == null)
            {
                Console.WriteLine("please choose between 1 and 2 only");
                continue;
            }

            if (input.Equals("1"))
            {
                Console.WriteLine("enter the string to console");
                ConsoleInputHandler inputHandler;
                try
                {
                    inputHandler = new ConsoleInputHandler();
                    var board = new Board(inputHandler);
                    Console.WriteLine("the initial board is:");
                    board.PrintBoard();
                    stopwatch.Start();
                    Console.WriteLine("the board after Constraint is:");
                    ConstraintPropagation.RunConstraintPropagation(board);
                    board.PrintBoard();
                   
                    var Solver = new AntSolver(board,Constants.GLOBAL_PHER_UPDATE, Constants.BEST_PHER_EVAP, Constants.NUM_OF_ANTS);
                    var solved = Solver.Solve(Constants.LOCAL_PHER_UPDATE, Constants.GREEDINESS);
                    stopwatch.Stop();
                    
                    solved.PrintBoard();
                    inputHandler.PrintString(solved.BoardToString());

                    Console.WriteLine();
                    Console.WriteLine("the amount of time it took to solve: " + stopwatch.ElapsedMilliseconds);
                    stopwatch.Reset();
                }
                catch (BoardSizeException e)
                {
                    Console.WriteLine(e.Message);
                }
                catch (InvalidCharException e2)
                {
                    Console.WriteLine(e2.Message);
                }
                catch (InvalidBoardException e3)
                {
                    Console.WriteLine(e3.Message);
                }

                continue;
            }

            if (input.Equals("2"))
            {
        

                try
                {
                    Console.WriteLine("enter file path");
                    FileInputHandler inputHandler;
                    inputHandler = new FileInputHandler();
                    var board = new Board(inputHandler);
                    Console.WriteLine("the initial board is:");
                    board.PrintBoard();

                    Console.WriteLine("the board after Constraint is:");
                    stopwatch.Start();
                    ConstraintPropagation.RunConstraintPropagation(board);
                    board.PrintBoard();

                    var Solver = new AntSolver(board, Constants.GLOBAL_PHER_UPDATE, Constants.BEST_PHER_EVAP, Constants.NUM_OF_ANTS);
                    var solved = Solver.Solve(Constants.LOCAL_PHER_UPDATE, Constants.GREEDINESS);
                    stopwatch.Stop();

                    inputHandler.PrintString(solved.BoardToString());
                    solved.PrintBoard();
                    Console.WriteLine();
                    Console.WriteLine("the amount of time it took to solve: " + stopwatch.ElapsedMilliseconds);
                    stopwatch.Reset();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                continue;
            }

            if (input.Equals("3"))
            {
                Console.WriteLine("Bye Bye");
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("please choose between 1 and 2 only");
            }
        }
    }
}