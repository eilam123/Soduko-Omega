using System.Diagnostics;
using Soduko_Omega.Ants;
using Soduko_Omega.IO;

namespace Soduko_Omega;

/*
public  class UI
{
    private static UI _instance;

    private UI()
    {
    }

    public static UI Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new UI();
            }

            return _instance;
        }
    }

    public static void Game()
    {
        while (true)
        {
            Stopwatch stopwatch = new Stopwatch();
            Console.WriteLine("Press 1 to take the input from the console");
            Console.WriteLine("Press 2 to take the input from the file");
            string? input = Console.ReadLine();
            if (input == null)
            {
                Console.WriteLine("please choose between 1 and 2 only");
                continue;
            }

            if (input.Equals("1"))
            {
                Console.WriteLine("enter the string to console");
                ConsoleInputHandler inputHandler;
                inputHandler = new ConsoleInputHandler();
                AntBoard? board = new AntBoard(inputHandler);
                Console.WriteLine("the initial board is:");
                board.PrintBoard();
                stopwatch.Start();
                Console.WriteLine("the board after Constraint is:");
                SodukoSolver.RunConstraintPropagation(board);
                board.PrintBoard();

                AntSolver Solver = new AntSolver(board, 0.9, 0.05, 30);
                AntBoard solved = Solver.Solve(0.1, 0.9);
                solved.PrintBoard();
                inputHandler.PrintString(solved.BoardToString());
                stopwatch.Stop();
                Console.WriteLine();
                Console.WriteLine("the amount of time it took to solve: " + stopwatch.ElapsedMilliseconds);
                stopwatch.Reset();
                continue;
            }

            if (input.Equals("2"))
            {
                Console.WriteLine("enter file path");
                FileInputHandler inputHandler;
                inputHandler = new FileInputHandler();
                AntBoard? board = new AntBoard(inputHandler);
                Console.WriteLine("the initial board is:");
                board.PrintBoard();

                Console.WriteLine("the board after Constraint is:");
                stopwatch.Start();
                SodukoSolver.RunConstraintPropagation(board);
                board.PrintBoard();

                AntSolver Solver = new AntSolver(board, 0.9, 0.05, 30);
                AntBoard solved = Solver.Solve(0.1, 0.9);
                inputHandler.PrintString(solved.BoardToString());
                stopwatch.Stop();
                Console.WriteLine();
                Console.WriteLine("the amount of time it took to solve: " + stopwatch.ElapsedMilliseconds);
                stopwatch.Reset();
            }
            else
            {
                Console.WriteLine("please choose between 1 and 2 only");
            }
        }
    }
}
*/

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
                // custom the exceptions;
                try
                {
                    inputHandler = new ConsoleInputHandler();
                    var board = new Board(inputHandler);
                    Console.WriteLine("the initial board is:");
                    board.PrintBoard();
                    stopwatch.Start();
                    Console.WriteLine("the board after Constraint is:");
                    SodukoSolver.RunConstraintPropagation(board);
                    board.PrintBoard();
                    SodukoSolver.RunConstraintPropagation(board);
                    board.PrintBoard();

                    var Solver = new AntSolver(board, 0.9, 0.05, 10);
                    var solved = Solver.Solve(0.1, 0.9);
                    stopwatch.Stop();
                    solved.PrintBoard();
                    inputHandler.PrintString(solved.BoardToString());

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

            if (input.Equals("2"))
            {
                // custom the exceptions;
                // catch custom exceptions for the file input handler wrong file path

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
                    SodukoSolver.RunConstraintPropagation(board);
                    board.PrintBoard();

                    var Solver = new AntSolver(board, 0.9, 0.05, 15);
                    var solved = Solver.Solve(0.1, 0.9);
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