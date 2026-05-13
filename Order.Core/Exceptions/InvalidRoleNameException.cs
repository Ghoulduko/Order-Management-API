using System.Runtime.Serialization;

namespace Order.Core.Exceptions;

public class InvalidRoleNameException : Exception
{
    public InvalidRoleNameException()
    {
    }

    protected InvalidRoleNameException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public InvalidRoleNameException(string? message) : base(message)
    {
    }

    public InvalidRoleNameException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}