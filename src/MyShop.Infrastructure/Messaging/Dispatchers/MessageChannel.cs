using MyShop.Application.Abstractions;
using MyShop.Infrastructure.Messaging.Dispatchers.Interfaces;
using System.Threading.Channels;

namespace MyShop.Infrastructure.Messaging.Dispatchers;
internal sealed class MessageChannel : IMessageChannel
{
    private readonly Channel<IMessage> _messages = Channel.CreateUnbounded<IMessage>();

    public ChannelReader<IMessage> Reader => _messages.Reader;
    public ChannelWriter<IMessage> Writer => _messages.Writer;
}
