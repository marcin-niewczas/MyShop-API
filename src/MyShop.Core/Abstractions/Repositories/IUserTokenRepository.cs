using MyShop.Core.Models.Users;

namespace MyShop.Core.Abstractions.Repositories;
public interface IUserTokenRepository : IBaseReadRepository<UserToken>, IBaseWriteRepository<UserToken>
{
}
