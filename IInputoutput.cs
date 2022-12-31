namespace Soduko_Omega;

public interface IInputoutput
{
    void ReadInput(int boardSize);
    bool ValidateInput(int boardSize);
}