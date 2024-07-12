using MyShop.Core.Exceptions;
using MyShop.Core.HelperModels;
using MyShop.Core.Models.BaseEntities;

namespace MyShop.Core.Models.Users;
public sealed class UserToken : BaseTimestampEntity
{
    public Browser Browser { get; private set; }
    public string? BrowserVersion { get; private set; }
    public OS OperatingSystem { get; private set; }
    public bool IsMobile { get; private set; }
    public string RefreshToken { get; private set; } = default!;
    public DateTimeOffset ExpiryRefreshTokenDate { get; private set; }
    public Guid UserId { get; private set; }
    public User User { get; private set; } = default!;

    private UserToken() { }

    public UserToken(
        string securedRefreshToken,
        PlatformInfo platformInfo,
        DateTimeOffset expiryRefreshTokenDate,
        Guid userId
        )
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(securedRefreshToken, nameof(securedRefreshToken));
        ArgumentNullException.ThrowIfNull(platformInfo, nameof(platformInfo));

        if (expiryRefreshTokenDate == default)
        {
            throw new ArgumentException($"Argument {nameof(expiryRefreshTokenDate)} cannot be default.", nameof(expiryRefreshTokenDate));
        }

        if (userId == default)
        {
            throw new ArgumentException($"Argument {nameof(userId)} cannot be default.", nameof(userId));
        }

        Browser = platformInfo.Browser;
        BrowserVersion = platformInfo.BrowserVersion;
        OperatingSystem = platformInfo.OperatingSystem;
        IsMobile = platformInfo.IsMobile;
        ExpiryRefreshTokenDate = expiryRefreshTokenDate;
        UserId = userId;
        RefreshToken = securedRefreshToken;
    }


    public UserToken Update(string securedRefreshToken, DateTimeOffset expiryRefreshTokenDate, PlatformInfo platformInfo, DateTimeOffset? now = null)
    {
        ArgumentNullException.ThrowIfNull(platformInfo, nameof(platformInfo));
        ArgumentException.ThrowIfNullOrWhiteSpace(securedRefreshToken, nameof(securedRefreshToken));

        if (expiryRefreshTokenDate == default)
        {
            throw new ArgumentException($"Argument {nameof(expiryRefreshTokenDate)} cannot be default.", nameof(expiryRefreshTokenDate));
        }

        //if (Browser != platformInfo.Browser ||
        //    OperatingSystem != platformInfo.OperatingSystem ||
        //    IsMobile != platformInfo.IsMobile)
        //{
        //    throw new BadRequestException("Invalid refresh token.");
        //}

        now ??= DateTimeOffset.UtcNow;

        if (ExpiryRefreshTokenDate < now)
        {
            throw new BadRequestException("Invalid refresh token.");
        }

        RefreshToken = securedRefreshToken;
        ExpiryRefreshTokenDate = expiryRefreshTokenDate;
        BrowserVersion = platformInfo.BrowserVersion;

        return this;
    }
}
