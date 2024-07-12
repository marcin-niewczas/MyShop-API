namespace MyShop.Infrastructure.Payments.Exceptions;
internal sealed class MyShopPayClientException : PaymentClientException
{
    public MyShopPayClientException() : base(null)
    {
    }

    public MyShopPayClientException(string? message) : base(message)
    {
    }
}
