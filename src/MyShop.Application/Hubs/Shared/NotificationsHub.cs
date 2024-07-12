using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MyShop.Application.Hubs.Shared.Interfaces;
using MyShop.Application.Utils;

namespace MyShop.Application.Hubs.Shared;
[Authorize(Policy = PolicyNames.HasCustomerPermission)]
public sealed class NotificationsHub : Hub<INotificationsHub>
{
    public override async Task OnConnectedAsync()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId!, Context.UserIdentifier!);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId!, Context.UserIdentifier!);
        await base.OnDisconnectedAsync(exception);
    }
}
