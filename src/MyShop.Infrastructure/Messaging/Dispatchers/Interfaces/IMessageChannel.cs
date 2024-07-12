using MyShop.Application.Abstractions;
using System.Threading.Channels;

namespace MyShop.Infrastructure.Messaging.Dispatchers.Interfaces;
internal interface IMessageChannel
{
    ChannelReader<IMessage> Reader { get; }
    ChannelWriter<IMessage> Writer { get; }
}
