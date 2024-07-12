using System.Net;

namespace MyShop.Core.Exceptions;
public sealed class BadRequestException : CustomException
{
    public override int StatusCode { get; } = (int)HttpStatusCode.BadRequest;

    public BadRequestException() : base(null)
    {
    }

    public BadRequestException(string? message) : base(message)
    {
    }
}
