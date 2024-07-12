using Microsoft.AspNetCore.Mvc;

namespace MyShop.Application.Commands.ECommerce.Orders;
public sealed record CancelOrderEc(
    [FromRoute] Guid Id
    ) : ICommand;
