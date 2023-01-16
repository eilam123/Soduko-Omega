using Soduko_Omega.Exceptions;

namespace Soduko_Omega.IO;

public class FileInputHandler : IInputoutput

{
    private string? _filePath;
    private string? _input;

    public FileInputHandler()
    {
        ReadInput();
    }
    /// <summary>
    /// This method reads the input from a file. It prompts the user for a file path, reads the file and calls the ValidateInput method
    /// </summary>
    public void ReadInput()
    {
        _filePath = Console.ReadLine();
        _input = File.ReadAllText(_filePath);
        ValidateInput();
    }
    /// <summary>
    /// This method validates the input. It checks if the input is null, if it has a perfect fourth root, and if it only contains valid characters
    /// </summary>
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
    /// <summary>
    /// This method returns the input string
    /// </summary>
    /// <returns>Input string</returns>
    public override string? ToString()
    {
        return _input;
    }
    /// <summary>
    /// This method takes a string parameter and writes it to the file specified by _filePath
    /// </summary>
    /// <param name="solved">string to write</param>
    public void PrintString(string solved)
    {
        File.WriteAllText(_filePath, solved);
    }
}