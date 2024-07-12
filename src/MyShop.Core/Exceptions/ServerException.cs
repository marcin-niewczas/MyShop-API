namespace MyShop.Core.Exceptions;
public sealed class ServerException(
    string? message
    ) : Exception(message);
