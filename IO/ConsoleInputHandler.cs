using Soduko_Omega.Exceptions;

namespace Soduko_Omega.IO;

public class ConsoleInputHandler : IInputoutput

{
    private string? _input;

    public ConsoleInputHandler()
    {
        ReadInput();
    }

    public void ReadInput()
    {
        _input = Console.ReadLine();
        ValidateInput();
    }

    //add doxumentation
    public void ValidateInput()
    {
        if (_input == null) throw new InvalidBoardException("can't enter empty board");

        if (Math.Sqrt(Math.Sqrt(_input.Length)) - Math.Floor(Math.Sqrt(Math.Sqrt(_input.Length))) != 0)
            throw new BoardSizeException(
                "Invalid size of input you should enter input that its lenght has perfect 4th root");


        foreach (var c in _input)
            if (!(c >= '0' && c <= '0' + Math.Sqrt(_input.Length)))
                throw new InvalidCharException();
    }

    public override string? ToString()
    {
        return _input;
    }

    public void PrintString(string solved)
    {
        Console.WriteLine(solved);
    }
}