namespace MyShop.Application.Commands.Account.Users;
public sealed record RemoveRegisteredUserAddressAc(
    Guid Id
    ) : ICommand;
