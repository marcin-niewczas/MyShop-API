using System.Net;

namespace MyShop.Core.Exceptions;
public sealed class ForbiddenException : CustomException
{
    public override int StatusCode => (int)HttpStatusCode.Forbidden;

    public ForbiddenException() : base(null)
    {
    }

    public ForbiddenException(string? message) : base(message)
    {
    }
}
