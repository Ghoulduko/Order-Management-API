using System.Runtime.Serialization;

namespace Order.Core.Exceptions;

public class InsufficientStockException : Exception
{
    public InsufficientStockException()
    {
    }

    protected InsufficientStockException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public InsufficientStockException(string? message) : base(message)
    {
    }

    public InsufficientStockException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}