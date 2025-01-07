namespace Ordering.Domain.Exceptions;

public class DomainException : Exception
{
    public DomainException(string messsage) : base($"Domain exception: \"{messsage}\" throw from Domain Layer. ")
    {
        
    }
}
