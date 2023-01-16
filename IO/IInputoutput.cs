namespace Soduko_Omega;

public interface IInputoutput
{
    void ReadInput();
    void ValidateInput();
    string? ToString();
    void PrintString(string solved);
}