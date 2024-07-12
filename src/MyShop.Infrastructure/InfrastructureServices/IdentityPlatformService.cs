using Microsoft.AspNetCore.Http;
using MyShop.Core.HelperModels;

namespace MyShop.Infrastructure.InfrastructureServices;
internal interface IIdentifyPlatformService
{
    PlatformInfo GetRequestPlatformInfo();
}

internal sealed class IdentifyPlatformService(IHttpContextAccessor httpContextAccessor) : IIdentifyPlatformService
{
    private readonly HttpContext _httpContext = httpContextAccessor.HttpContext
        ?? throw new ArgumentNullException(nameof(httpContextAccessor));

    public PlatformInfo GetRequestPlatformInfo()
    {
        var userAgent = _httpContext.Request.Headers.UserAgent.ToString();

        return new(userAgent);
    }
}
