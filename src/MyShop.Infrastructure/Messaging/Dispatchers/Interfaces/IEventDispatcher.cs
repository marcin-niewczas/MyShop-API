using MyShop.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Infrastructure.Messaging.Dispatchers.Interfaces;
internal interface IEventDispatcher
{
    Task PublishAsync(IEvent @event, CancellationToken cancellationToken = default);
}
