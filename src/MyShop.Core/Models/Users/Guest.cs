using MyShop.Core.ValueObjects.Users;

namespace MyShop.Core.Models.Users;
public sealed class Guest : User
{
    public Guest() : base(UserRole.Guest)
    {
    }
}
