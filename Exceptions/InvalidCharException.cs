namespace Soduko_Omega.Exceptions;

public class InvalidCharException : Exception
{
    public InvalidCharException() : base("Whoops it seems like there is char that doesn't belong to the board")
    {
    }

    public InvalidCharException(string message) : base(message)
    {
    }

    public InvalidCharException(string message, Exception inner) : base(message, inner)
    {
    }
}