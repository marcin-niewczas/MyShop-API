namespace MyShop.Infrastructure.Payments.Exceptions;
internal abstract class PaymentClientException : Exception
{
    public PaymentClientException() : base(null)
    {
    }

    public PaymentClientException(string? message) : base(message)
    {
    }
}
