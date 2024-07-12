using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace MyShop.Application.Hubs.Providers;
internal sealed class NameBasedUserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        return connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}
