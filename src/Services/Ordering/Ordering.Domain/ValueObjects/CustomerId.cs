namespace Ordering.Domain.ValueObjects;

public class CustomerId
{
    public Guid Value { get; }
    private CustomerId(Guid value) => Value = value;

    public static CustomerId Off(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value == Guid.Empty)
        {
            throw new DomainException("CustomerId cannot be empty.");
        }

        return new CustomerId(value);
    }
}
