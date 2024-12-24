namespace BuildingBlock.Exceptions;

public class InternalServerException : Exception
{
    public InternalServerException(string message) : base(message)
    {            
    }

    public InternalServerException(string message, string detail) : base(message)
    {
        Details = detail;
    }

    public string Details { get; }

}
