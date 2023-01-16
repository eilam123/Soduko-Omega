namespace Soduko_Omega.Exceptions;

public class BoardSizeException : Exception
{
    public BoardSizeException() : base("AntBoard size is invalid")
    {
    }

    public BoardSizeException(string message) : base(message)
    {
    }

    public BoardSizeException(string message, Exception inner) : base(message, inner)
    {
    }
}