using MyShop.Core.HelperModels;
using MyShop.Core.Models.BaseEntities;
using MyShop.Core.Models.Orders;
using MyShop.Core.Models.ShoppingCarts;
using MyShop.Core.Utils;
using MyShop.Core.ValueObjects.Users;

namespace MyShop.Core.Models.Users;
public abstract class User : BaseTimestampEntity
{
    public UserRole Role { get; private set; } = default!;
    public ShoppingCart ShoppingCart { get; protected set; } = default!;
    public IReadOnlyCollection<UserToken> UserTokens => _userTokens;
    private readonly List<UserToken> _userTokens = default!;
    public IReadOnlyCollection<Order> Orders => _orders;
    private readonly List<Order> _orders = default!;

    protected User(UserRole userRole)
    {
        ShoppingCart = new(Id);
        _userTokens = [];
        Role = userRole ?? throw new ArgumentNullException(nameof(userRole));
    }

    public UserToken AddUserToken(string securedRefreshToken, PlatformInfo platformInfo, DateTimeOffset expiryRefreshTokenDate)
    {
        ArgumentNullException.ThrowIfNull(platformInfo, nameof(platformInfo));
        ArgumentNullException.ThrowIfNull(_userTokens, nameof(_userTokens));

        var userToken = new UserToken(
            securedRefreshToken,
            platformInfo,
            expiryRefreshTokenDate,
            Id
            );

        _userTokens.Add(userToken);

        return userToken;
    }

    public void ClearUserTokens()
    {
        ArgumentNullException.ThrowIfNull(_userTokens, nameof(_userTokens));

        _userTokens.Clear();
    }

    public void ClearUserTokens(Guid expectedUserTokenId)
    {
        ArgumentNullException.ThrowIfNull(_userTokens, nameof(_userTokens));

        _userTokens.RemoveAll(t => t.Id != expectedUserTokenId);
    }

    public int RemoveExpiredTokens(DateTimeOffset? now)
    {
        ArgumentNullException.ThrowIfNull(_userTokens, nameof(_userTokens));

        now ??= DateTimeOffset.Now;

        var expiredTokens = _userTokens.Where(t => t.ExpiryRefreshTokenDate <= now).ToList();

        if (expiredTokens.IsNullOrEmpty())
        {
            return 0;
        }

        return expiredTokens
             .Select(_userTokens.Remove)
             .Count(r => r);
    }
}
