namespace Soduko_Omega.Exceptions;

public class InvalidBoardException : Exception
{
    public InvalidBoardException() : base("This board has double values in the same row/col/box")
    {
    }

    public InvalidBoardException(string message) : base(message)
    {
    }

    public InvalidBoardException(string message, Exception inner) : base(message, inner)
    {
    }
}