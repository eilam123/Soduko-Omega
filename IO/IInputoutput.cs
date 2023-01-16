namespace Soduko_Omega;
/// <summary>
/// This interface defines the methods that classes used for input/output should implement
/// </summary>
public interface IInputoutput
{
    /// <summary>
    /// This method reads input from the source specified by the implementing class.
    /// </summary>
    void ReadInput();
    /// <summary>
    /// This method validates the input. It checks if the input is valid and throws exceptions if it's not.
    /// </summary>
    void ValidateInput();
    /// <summary>
    /// This method returns the input as a string.
    /// </summary>
    /// <returns>Input as a string</returns>
    string? ToString();
    /// <summary>
    /// This method takes a string parameter and writes it to the output specified by the implementing class.
    /// </summary>
    /// <param name="solved">string to write</param>
    void PrintString(string solved);
}