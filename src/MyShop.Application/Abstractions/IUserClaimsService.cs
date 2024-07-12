using MyShop.Core.HelperModels;

namespace MyShop.Application.Abstractions;
public interface IUserClaimsService
{
    UserClaimsData GetUserClaimsData();
}
