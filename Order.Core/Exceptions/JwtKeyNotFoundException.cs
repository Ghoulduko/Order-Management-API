using System.Runtime.Serialization;

namespace Order.Core.Exceptions;

public class JwtKeyNotFoundException : Exception
{
    public JwtKeyNotFoundException()
    {
    }

    protected JwtKeyNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public JwtKeyNotFoundException(string? message) : base(message)
    {
    }

    public JwtKeyNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}