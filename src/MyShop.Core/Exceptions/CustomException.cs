namespace MyShop.Core.Exceptions;
public abstract class CustomException(
    string? message
    ) : Exception(message)
{
    public abstract int StatusCode { get; }
}
