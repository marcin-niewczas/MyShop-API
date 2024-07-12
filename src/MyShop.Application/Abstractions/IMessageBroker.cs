namespace MyShop.Application.Abstractions;
public interface IMessageBroker
{
    Task PublishAsync(params IMessage[] messages);
}
